using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.ResponseCompression;
using Presentation.Config;
using Presentation.Filters;

[assembly: HostingStartup(typeof(ConfigurePresentation))]

namespace Presentation.Config;

public class ConfigurePresentation : IHostingStartup
{
    private static bool _configured;

    private static readonly string[] CompressionTypes = ["application/octet-stream"];

    public void Configure(IWebHostBuilder builder)
    {
        if (_configured) return;
        _configured = true;

        builder.ConfigureServices(services =>
        {
            services.AddAntiforgery();

            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("/db");

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = false;
            });

            services
                .AddControllers(options =>
                {
                    options.Filters.Add<SetClientIpAddressFilter>();
                    options.Filters.Add<FluentValidationFilter>();
                    options.Filters.Add<ResponseTimeFilter>();
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            // this is bad, because we want to use ProblemDetails for status code errors.
            // .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

            // TODO SignalR
            // services.AddSignalR(o => { o.EnableDetailedErrors = env.IsDevelopment(); });

            services.AddResponseCaching();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(CompressionTypes);
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });
        });
    }
}