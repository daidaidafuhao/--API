using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebAPI.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 检查是否是需要豁免的路径（如登录接口）
            if (IsExemptPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

            // 检查请求头中是否包含token
            if (!context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsJsonAsync(new { success = false, message = "缺少授权token" });
                return;
            }

            // 验证token是否有效
            if (!IsValidToken(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsJsonAsync(new { success = false, message = "无效的token" });
                return;
            }

            // token验证通过，继续处理请求
            await _next(context);
        }

        private bool IsExemptPath(PathString path)
        {
            // 定义不需要验证token的路径
            var exemptPaths = new[]
            {
                "/api/database/test-connection",
                "/api/Users/login",
                "/api/Users/loginAdmin",
                "/lib",  // 移除尾部斜杠以确保正确匹配
                "/js",   // 添加js目录
                "/css", // 添加css目录
                "/admin", // 添加admin目录
                "/login.html",
                "/index.html",
                "/",
                "/favicon.ico",
                "/wwwroot" // 添加wwwroot目录
                // 可以添加其他豁免路径
            };

            // 检查路径是否以任何豁免路径开头
            foreach (var exemptPath in exemptPaths)
            {
                if (path.Value?.StartsWith(exemptPath, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool IsValidToken(string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken) || !bearerToken.StartsWith("Bearer "))
                return false;

            var token = bearerToken.Substring("Bearer ".Length).Trim();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}