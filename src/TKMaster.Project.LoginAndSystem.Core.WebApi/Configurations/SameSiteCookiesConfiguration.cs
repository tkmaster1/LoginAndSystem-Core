using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations;

public static class SameSiteCookiesConfiguration
{
    #region Properties

    private const SameSiteMode Unspecified = (SameSiteMode)(-1);

    #endregion

    #region Methods

    public static IServiceCollection AddConfigureSameSiteCookies(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = Unspecified;
            options.OnAppendCookie = cookieContext =>
               CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
               CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        return services;
    }

    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
    {
        if (options.SameSite == SameSiteMode.None)
            options.SameSite = Unspecified;
    }

    #endregion
}
