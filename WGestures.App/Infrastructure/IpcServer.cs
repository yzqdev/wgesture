using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;
using Serilog;

namespace WGestures.App.Infrastructure;

/// <summary>
/// 命名管道 IPC：服务端常驻监听（接收二次启动/外部的命令），
/// 客户端用于向已运行的实例发送命令，实现单实例唤醒。
/// </summary>
internal static class IpcServer
{
    public const string PipeName = "WGestures_IPC_API";

    /// <summary>
    /// 启动服务端监听线程；收到命令后经 WindowsFormsSynchronizationContext 编组到调用方线程执行 onCommand。
    /// </summary>
    public static void Start(Action<string> onCommand)
    {
        var synCtx = new WindowsFormsSynchronizationContext();
        var pipeThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    using (var server = new NamedPipeServerStream(PipeName))
                    {
                        server.WaitForConnection();

                        Debug.WriteLine("Client Connected");
                        using (var reader = new StreamReader(server))
                        {
                            var cmd = reader.ReadLine();
                            Debug.WriteLine("Pipe CMD=" + cmd);

                            if (cmd != null)
                            {
                                synCtx.Post(s => onCommand(cmd), null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "IPC 管道异常");
                }
            }
        }, maxStackSize: 65536) { IsBackground = true };
        pipeThread.Start();
    }

    /// <summary>向已运行的实例发送命令（连接超时 1s，失败仅记录调试输出）。</summary>
    public static void PostCommand(string cmd)
    {
        try
        {
            using (var pipeClient = new NamedPipeClientStream(PipeName))
            {
                pipeClient.Connect(1000);
                using (var writer = new StreamWriter(pipeClient) { AutoFlush = true })
                {
                    writer.WriteLine(cmd);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("PostIpcCmd Error: " + ex.Message);
        }
    }
}
