using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations;

public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddDbContext<MeuContexto>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    #region IdentityConfiguration

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AspNetCoreIdentityContextConnection")));

        //services.AddDefaultIdentity<UsuarioIdentity>()
        //        .AddRoles<IdentityRole>()
        //        .AddErrorDescriber<IdentityMensagensPortugues>()
        //        .AddEntityFrameworkStores<IdentityContext>()
        //        .AddDefaultTokenProviders();
        services.AddIdentity<UsuarioIdentity, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = false;
            options.User.AllowedUserNameCharacters = "aãâàábcçdeéêfgğhiíîìjklmnoöõôópqrsştuüúûùvwxyzAÃÂÀÁBCÇDEÉÊFGĞHIİÎÌJKLMNOÖÔÕÓPQRSŞTUÜÚÛÙVWXYZ0123456789-._@+/ ";
            options.Tokens.PasswordResetTokenProvider = "7DaysToken";
        })
           .AddDefaultUI()
           .AddErrorDescriber<IdentityMensagensPortugues>()
           .AddEntityFrameworkStores<IdentityContext>()
           .AddDefaultTokenProviders()
           .AddTokenProvider<DataProtectorTokenProvider<UsuarioIdentity>>("7DaysToken");

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
        {
            opt.TokenLifespan = TimeSpan.FromDays(7);
            opt.Name = "7DaysToken";
        });

        // JWT
        var appSettingsSection = configuration.GetSection("AuthorizationSettings");
        services.Configure<AuthorizationSettings>(appSettingsSection);

        var appSettings = appSettingsSection.Get<AuthorizationSettings>();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = appSettings.ValidOn,
                ValidIssuer = appSettings.Issuer,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    #endregion
}
