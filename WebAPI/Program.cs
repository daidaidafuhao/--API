using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Repositories;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.HttpLogging;
using WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "WebAPI")
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.File("Logs/api-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {RequestPath} {RequestMethod} {StatusCode} {Elapsed}ms {Message:lj}{NewLine}{RequestBody}{NewLine}{ErrorDetails}{NewLine}{Exception:l}")
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// 配置JWT认证服务
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("X-Response-Time");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Enable HTTP logging middleware
app.UseHttpLogging();

// Enable static files middleware
app.UseStaticFiles();

// Configure token validation middleware to skip static files
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/lib") || 
        context.Request.Path.StartsWithSegments("/admin"))
    {
        await next();
        return;
    }
    await next();
});

// 获取详细的错误信息
async Task<string> GetErrorDetails(HttpContext context)
{
    if (context.Response.StatusCode < 400) return "";
    
    try
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var errorMessage = await reader.ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return $"Error: {context.Response.StatusCode} - {errorMessage}";
        }
    }
    catch
    {
        // 如果无法读取响应体，返回基本错误信息
    }
    
    return $"Error: {context.Response.StatusCode} - No detailed error message available";
}

// Add custom middleware to enrich log context
app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    
    // 创建一个新的响应体流来捕获响应
    var originalBody = context.Response.Body;
    var memStream = new MemoryStream();
    context.Response.Body = memStream;
    
    try
    {
        // 读取请求体
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        Log.ForContext("RequestPath", context.Request.Path)
           .ForContext("RequestMethod", context.Request.Method)
           .ForContext("RequestBody", requestBody)
           .Information("Starting request");

        await next();

        sw.Stop();
        var errorDetails = await GetErrorDetails(context);
        Log.ForContext("RequestPath", context.Request.Path)
           .ForContext("RequestMethod", context.Request.Method)
           .ForContext("StatusCode", context.Response.StatusCode)
           .ForContext("Elapsed", sw.ElapsedMilliseconds)
           .ForContext("ErrorDetails", errorDetails)
           .Information("Request completed");

        // 确保在使用完MemoryStream后再复制并释放
        if (memStream != null && memStream.CanRead)
        {
            try
            {
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);
            }
            catch (ObjectDisposedException)
            {
                // 如果流已经被释放，记录日志但不抛出异常
                Log.Warning("Memory stream was already disposed when attempting to copy response");
            }
        }
    }
    catch (Exception ex)
    {
        sw.Stop();
        Log.ForContext("RequestPath", context.Request.Path)
           .ForContext("RequestMethod", context.Request.Method)
           .ForContext("StatusCode", 500)
           .ForContext("Elapsed", sw.ElapsedMilliseconds)
           .Error(ex, "Request failed");
        throw;
    }
});

app.MapControllers();

// Configure token validation middleware to skip static files
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/lib") || 
        context.Request.Path.StartsWithSegments("/admin"))
    {
        await next();
        return;
    }
    await next();
});

app.UseMiddleware<TokenValidationMiddleware>();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
