using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WGestures.Common.Product;

/// <summary>
/// 检查新版本
/// </summary>
public class VersionChecker : IDisposable
{
    // HttpClient 在整个应用程序生命周期内应该重用
    private static readonly HttpClient _defaultClient = new(new HttpClientHandler { Proxy = null });
    
    private readonly HttpClient _httpClient;
    private readonly string _url;
    private readonly int _timeoutSeconds;
    private CancellationTokenSource? _cts;
    private bool _isBusy;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="url">检查更新的 URL</param>
    /// <param name="timeOutSeconds">超时时间（秒）</param>
    /// <param name="customClient">可选：允许外部注入 HttpClient（利于单元测试）</param>
    public VersionChecker(string url, int timeOutSeconds = 15, HttpClient? customClient = null)
    {
        _url = url ?? throw new ArgumentNullException(nameof(url));
        _timeoutSeconds = timeOutSeconds;
        _httpClient = customClient ?? _defaultClient;
    }

    // 使用现代的 C# 属性简化写法
    public event Action<VersionInfo?>? Finished;
    public event Action? Canceled;
    public event Action<Exception>? ErrorHappened;

    public bool IsBusy => _isBusy;

    /// <summary>
    /// 异步开始检查版本
    /// </summary>
    public async void CheckAsync()
    {
        if (_isBusy) return;

        _isBusy = true;
        _cts = new CancellationTokenSource();
        // 设置超时机制
        _cts.CancelAfter(TimeSpan.FromSeconds(_timeoutSeconds));

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // 直接请求并将 JSON 反序列化为对象（使用 .NET 强大的原生 JSON 库）
            var versionInfo = await _httpClient.GetFromJsonAsync<VersionInfo>(_url, _cts.Token);
            
            Finished?.Invoke(versionInfo);
        }
        catch (OperationCanceledException)
        {
            // 如果是因为 _cts.Token 被取消（包括超时），会抛出此异常
            Canceled?.Invoke();
           
        }
        catch (Exception ex)
        {
            ErrorHappened?.Invoke(ex);
        }
        finally
        {
            stopwatch.Stop();
            Debug.WriteLine($"CheckAsync Time used: {stopwatch.Elapsed.TotalSeconds} seconds");
            
            _isBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    /// <summary>
    /// 取消当前正在进行的请求
    /// </summary>
    public void Cancel()
    {
        _cts?.Cancel();
    }

    public void Dispose()
    {
        Cancel();
        _cts?.Dispose();
        // 注意：不要在这里释放 _httpClient，因为它是共享的静态实例
    }
}