using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades;

public class UserAppService : IUserAppService
{
    #region Properties

    private readonly IHttpContextAccessor _accessor;

    #endregion

    #region Construtor

    public UserAppService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    #endregion

    #region Methods

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
    }

    public bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext.User.IsInRole(role);
    }

    public string ObterUserToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserToken() : "";
    }

    public string ObterUserRefreshToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRefreshToken() : "";
    }

    public IEnumerable<Claim> GetClaimsIdentity()
    {
        return _accessor.HttpContext.User.Claims;
    }

    #endregion
}

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null) throw new ArgumentException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null) throw new ArgumentException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.Email);
        return claim?.Value;
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal == null) throw new ArgumentException(nameof(principal));

        var claim = principal.FindFirst("JWT");
        return claim?.Value;
    }

    public static string GetUserRefreshToken(this ClaimsPrincipal principal)
    {
        if (principal == null) throw new ArgumentException(nameof(principal));

        var claim = principal.FindFirst("RefreshToken");
        return claim?.Value;
    }
}