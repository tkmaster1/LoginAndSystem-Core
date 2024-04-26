using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using TKMaster.Project.Common.Util.Configuration;
using TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations.Filters;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations;

public static class CommonsConfiguration
{
    public static IServiceCollection AddApiCommonsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(CustomActionFilterConfig));

        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = false;
        });

        services.AddHttpContextAccessor();
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddResponseCaching();

        // services.Configure<EmailConfiguracoes>(configuration.GetSection("EmailConfiguracoes"));

        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
        });

        services.AddDistributedMemoryCache();

        services.AddConfigureSameSiteCookies();

        services.AddCors();

        services.AddMvc(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        IdentityModelEventSource.ShowPII = true;

        var provider = services.BuildServiceProvider();

        return services;
    }

    public static IApplicationBuilder UseApiCommonsConfiguration(this IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment() || env.IsProduction())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Acesso Login Identity e outros - Core API"));
        }
        else
        {
            //app.UseExceptionHandler("/Home/Error/500");
            //app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCookiePolicy();

        app.UseCors
         (
             c =>
             {
                 c.AllowAnyOrigin();
                 c.AllowAnyHeader();
                 c.AllowAnyMethod();
             }
         );

        app.UseIdentityConfiguration();

        app.UseLoggingConfiguration(loggerFactory);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseStaticFiles();

        app.UseGlobalizationConfig();

        return app;
    }
}
