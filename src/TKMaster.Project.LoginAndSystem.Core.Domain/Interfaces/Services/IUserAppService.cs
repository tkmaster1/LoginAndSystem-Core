using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

public interface IUserAppService
{
    string Name { get; }
    Guid GetUserId();

    string GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    string ObterUserToken();

    string ObterUserRefreshToken();

    IEnumerable<Claim> GetClaimsIdentity();
}