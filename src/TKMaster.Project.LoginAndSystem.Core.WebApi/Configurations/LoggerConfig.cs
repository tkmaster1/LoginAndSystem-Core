using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TKMaster.Project.Common.Util.Configuration;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations;

public static class LoggerConfig
{
    public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        loggerFactory.UseLoggerFactory(Path.GetFullPath(Directory.GetCurrentDirectory() + "\\Log\\"), $"LoginAndSystem_core_{DateTime.Now.ToString("yyyyMMdd")}.txt");
        loggerFactory.CreateLogger("LoginAndSystem-core").LogError("init");

        app.UseFileServer(new FileServerOptions()
        {
            FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), @"Log")),
            RequestPath = new PathString("/app-log"),
            EnableDirectoryBrowsing = true,
            EnableDefaultFiles = true
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Log")),
            RequestPath = new PathString("/app-log"),
        });

        return app;
    }
}
