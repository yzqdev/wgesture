using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace WGestures.Common.Config;

public class PlistConfig
{
    //public string FileVersion
    //{
    //    get {
    //        return Get<string>("$$FileVersion", null);
    //    }
    //    set
    //    {
    //        Set("$$FileVersion",value);
    //    }
    //}
    public ConfigData Dict { get; set; }

    public string PlistPath { get; set; }

    /// <summary>
    /// 创建一个空的Config
    /// </summary>
    public PlistConfig(){}

    /// <summary>
    /// 创建并指定要加载或保存的plist文件位置
    /// </summary>
    /// <param name="plistPath">Plist path.</param>
    public PlistConfig(string plistPath)
    {
        PlistPath = plistPath;
        Load();
    }

    public PlistConfig(Stream stream, bool closeStream)
    {
        Load(stream,closeStream);
    }

    private void Load()
    {
        if (PlistPath == null)
            throw new InvalidOperationException("未指定需要加载的plist文件路径");
        if (!File.Exists(PlistPath))
        {
            return;
        }

        //StreamReader 必须释放：否则读句柄存活到 GC 终结化，
        //期间 Save() 对同一文件的写入会撞共享冲突（重试也无法恢复，直到句柄被 GC 关闭）
        using (StreamReader reader = new StreamReader(PlistPath))
        {
            Dict=JsonConvert.DeserializeObject<ConfigData>(reader.ReadToEnd());
        }
        //Dict = (Dictionary<string, object>)Plist.readPlist(PlistPath);
    }

    private void Load(Stream stream, bool closeStream = false)
    {
        if(stream == null || !stream.CanRead) throw new ArgumentException("stream");

        try
        {
            using (StreamReader reader = new StreamReader(PlistPath))
            {
                Dict = JsonConvert.DeserializeObject<ConfigData>(reader.ReadToEnd());
            }
        }
        catch (Exception)
        {
            if (closeStream && stream != null) stream.Close();
            throw;
        }

        //closeStream 语义：成功加载后也应关闭调用方传入的流
        if (closeStream && stream != null) stream.Close();
    }

    //Save 可能从不同线程调用（设置界面 UI 线程、手势解析线程的托盘事件），互斥避免自相冲突
    private static readonly object SaveLock = new object();

    public void Save()
    {
        if (PlistPath == null)
            throw new InvalidOperationException("未指定需要保存到的plist文件路径(PlistPath属性)");

        WriteDict(PlistPath, Dict);
    }

    private static void WriteDict(string path, ConfigData dict)
    {
        lock (SaveLock)
        {
            JsonSerializer serializer = new JsonSerializer();

            //配置文件可能被外部程序（杀软/索引器）短暂占用，共享冲突时小退避重试。
            //重试总时长需覆盖常见扫描窗口（实践上 >600ms 仍会撞），共 5 次约 1.5s
            const int maxRetries = 5;
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, dict);
                    }
                    return;
                }
                catch (IOException) when (attempt < maxRetries)
                {
                    Thread.Sleep(100 * (1 << (attempt - 1)));
                }
            }
        }
    }

    public void Import(PlistConfig config)
    {
        WriteDict(PlistPath, config.Dict);
    }

    public void Import(PlistConfig config1, PlistConfig config2)
    {
        throw new NotImplementedException();
    }
}
