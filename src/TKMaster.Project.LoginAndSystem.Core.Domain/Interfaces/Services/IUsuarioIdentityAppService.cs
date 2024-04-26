using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

public interface IUsuarioIdentityAppService : IDisposable
{
    #region Login

    //Task<ForgotPasswordResult> ForgotPassword(string email);

    //Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPassword); IUsuarioIdentityRepository

    Task<ResponseLoginUsuarioIdentity> GerarJwt(string email, AuthorizationSettings authorizationSettings);

    Task<SignInResult> PasswordSignIn(string userName, LoginUsuarioModel loginUser);

    Task<SignInResult> PasswordSignIn(UsuarioIdentity userIdentityEntity, LoginUsuarioModel loginUser);

    #endregion

    #region Usuários

    Task<UsuarioIdentity> FindByEmail(string email);

    Task<IdentityResult> CreateUserIdentity(UsuarioIdentity entity);

    Task<bool> UpdateUserIdentity(UsuarioIdentity userIdentityEntity);

    Task<bool> InactivateUserIdentity(string codigoUser);

    Task<bool> ReactivateUserIdentity(string codigoUser);

    Task<IdentityResult> ChangePasswordUserIdentity(UsuarioIdentityMudancaSenhaEntity mudancaSenhaModel);

    Task<UsuarioIdentity> GetUserIdentityById(string codigoUser);

    Task<Pagination<UsuarioIdentity>> GetListByFilter(UsuarioIdentityFilter filter);

    #endregion
}