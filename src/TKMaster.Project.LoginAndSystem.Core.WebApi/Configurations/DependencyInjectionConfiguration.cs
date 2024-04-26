using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;
using TKMaster.Project.LoginAndSystem.Core.Data.Repository;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Domain.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Service.Application;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Lifestyle.Transient => Uma instância para cada solicitação
        // Lifestyle.Singleton => Uma instância única para a classe (para servidor)
        // Lifestyle.Scoped => Uma instância única para o request

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<INotificador, Notificador>();
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        #region Application

        #region UserIdentity

        services.AddTransient<IUsuarioIdentityFacade, UsuarioIdentityFacade>();
        services.AddTransient<IPermissoesUsuarioIdentityFacade, PermissoesUsuarioIdentityFacade>();
        services.AddScoped<IMenuSistemaFacade, MenuSistemaFacade>();

        #endregion

        #endregion

        #region Domain

        #region UserIdentity

        services.AddTransient<IUsuarioIdentityAppService, UsuarioIdentityAppService>();
        services.AddTransient<IPermissoesUsuarioIdentityAppService, PermissoesUsuarioIdentityAppService>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<IMenuSistemaAppService, MenuSistemaAppService>();

        #endregion

        #endregion

        #region InfraData

        services.AddScoped<MeuContexto>();
        services.AddScoped<IdentityContext>();

        #region UserIdentity

        services.AddTransient<IUsuarioIdentityRepository, UsuarioIdentityRepository>();
        services.AddTransient<IPermissoesUsuarioIdentityRepository, PermissoesUsuarioIdentityRepository>();
        services.AddScoped<IMenuSistemaRepository, MenuSistemaRepository>();

        #endregion

        #endregion
    }
}