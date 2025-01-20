using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi;
using WebApi.Domain.Context;
using WebApi.Domain.Interfaces.Common;
using WebApi.Domain.Interfaces.Repositories;
using WebApi.Domain.Interfaces.Services;
using WebApi.Service.Common;
using WebApi.Service.Repositories;
using WebApi.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ICuttingDownAService, CuttingDownAService>();
builder.Services.AddScoped<ICuttingDownBService, CuttingDownBService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICabinRepository, CabineRepository>();
builder.Services.AddScoped<ICableRepository, CableRepository>();
builder.Services.AddScoped<ICuttingDownBRepository, CuttingDownBRepository>();
builder.Services.AddScoped<ICuttingDownARepository, CuttingDownARepository>();
builder.Services.AddScoped<ISTAProblemTypeRepository, STAProblemTypeRepository>();


builder.Services.AddDbContext<MyDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true;
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("user-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress?.ToString(),
            _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        var responseObject = new
        {
            Status = 429,
            Title = $"Too many requests {context.HttpContext.Request.Headers["X-Real-IP"]}",
            Detail = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? $"Please try again after {retryAfter.TotalMinutes} minute(s)."
                : "Please try again later.",
        };

        var jsonResponse = JsonConvert.SerializeObject(responseObject,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(jsonResponse, token);
    };
});


var app = builder.Build();

app.UseRateLimiter();
app.UseResponseCompression();
app.MapControllers();
app.Run();