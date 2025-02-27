using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UdpBroadcastController : ControllerBase
    {
        private readonly ILogger<UdpBroadcastController> _logger;
        private UdpClient _udpListener;
        private bool _isListening;
        private readonly int _port = 8888; // 默认监听端口

        public UdpBroadcastController(ILogger<UdpBroadcastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("start")]
        public IActionResult StartListening()
        {
            if (_isListening)
            {
                return BadRequest("UDP监听服务已经在运行");
            }

            try
            {
                _udpListener = new UdpClient(_port);
                _isListening = true;

                // 在后台任务中开始监听
                Task.Run(async () => await ListenForBroadcastsAsync());

                _logger.LogInformation($"UDP广播监听服务已启动，监听端口: {_port}");
                return Ok($"UDP广播监听服务已启动，监听端口: {_port}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动UDP监听服务时发生错误");
                return StatusCode(500, $"启动UDP监听服务时发生错误: {ex.Message}");
            }
        }

        [HttpPost("stop")]
        public IActionResult StopListening()
        {
            if (!_isListening)
            {
                return BadRequest("UDP监听服务未运行");
            }

            try
            {
                _isListening = false;
                _udpListener?.Close();
                _udpListener?.Dispose();
                _logger.LogInformation("UDP广播监听服务已停止");
                return Ok("UDP广播监听服务已停止");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止UDP监听服务时发生错误");
                return StatusCode(500, $"停止UDP监听服务时发生错误: {ex.Message}");
            }
        }

        private async Task ListenForBroadcastsAsync()
        {
            try
            {
                while (_isListening)
                {
                    var result = await _udpListener.ReceiveAsync();
                    _logger.LogInformation($"收到来自 {result.RemoteEndPoint} 的广播消息");

                    // 获取本机IP地址
                    string localIp = GetLocalIPAddress();
                    string response = $"Server IP: {localIp}, Port: {_port}";

                    // 发送响应
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await _udpListener.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);
                    _logger.LogInformation($"已发送响应到 {result.RemoteEndPoint}");
                }
            }
            catch (Exception ex) when (!_isListening) // 如果服务被停止，忽略异常
            {
                _logger.LogInformation("UDP监听服务已正常停止");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UDP监听服务发生错误");
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
    }
}