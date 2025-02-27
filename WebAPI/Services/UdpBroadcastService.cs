using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Services
{
    public class UdpBroadcastService : BackgroundService
    {
        private readonly ILogger<UdpBroadcastService> _logger;
        private readonly IConfiguration _configuration;
        private UdpClient _udpListener;
        private readonly int _port = 45678;
        private bool _isListening;

        public UdpBroadcastService(ILogger<UdpBroadcastService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // 从配置中获取HTTP服务端口
                var httpPort = _configuration.GetValue<int>("Kestrel:Endpoints:Http:Port", 5115);

                // 检查UDP端口是否被占用
                if (!IsPortAvailable(_port))
                {
                    _logger.LogError($"UDP端口 {_port} 已被占用，服务无法启动");
                    return;
                }

                _udpListener = new UdpClient(_port);
                _isListening = true;
                _logger.LogInformation($"UDP广播监听服务已启动，监听端口: {_port}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = await _udpListener.ReceiveAsync(stoppingToken);
                        _logger.LogInformation($"收到来自 {result.RemoteEndPoint} 的广播消息");

                        string localIp = GetLocalIPAddress();
                        string response = $"Server IP: {localIp}, Port: {httpPort}";
                        _logger.LogInformation($"响应 {response}");
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        await _udpListener.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);
                        _logger.LogInformation($"已发送响应到 {result.RemoteEndPoint}");
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "UDP监听服务发生错误");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动UDP监听服务时发生错误");
            }
            finally
            {
                _isListening = false;
                _udpListener?.Close();
                _udpListener?.Dispose();
                _logger.LogInformation("UDP广播监听服务已停止");
            }
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("未找到本机IPv4地址");
        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect("127.0.0.1", port);
                    return false; // 端口被占用
                }
            }
            catch (SocketException)
            {
                return true; // 端口可用
            }
        }
    }
}