using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Application;

public class UsuarioIdentityAppService : IUsuarioIdentityAppService
{
    #region Properties

    private readonly IUsuarioIdentityRepository _userIdentityRepository;
    private UserManager<UsuarioIdentity> _userManager;
    private SignInManager<UsuarioIdentity> _signInManager;

    #endregion

    #region Constructor

    public UsuarioIdentityAppService(IUsuarioIdentityRepository userIdentityRepository,
                               UserManager<UsuarioIdentity> userManager,
                               SignInManager<UsuarioIdentity> signInManager)
    {
        _userIdentityRepository = userIdentityRepository;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    #endregion

    #region Methods Public

    #region Login

    public async Task<ResponseLoginUsuarioIdentity> GerarJwt(string email, AuthorizationSettings authorizationSettings)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        var identityClaims = await ObterClaimsUsuario(claims, user);
        var encodedToken = CodificarToken(identityClaims, authorizationSettings);

        return ObterRespostaToken(encodedToken, user, claims, authorizationSettings);
    }

    public async Task<SignInResult> PasswordSignIn(string userName, LoginUsuarioModel loginUser)
        => await _signInManager.PasswordSignInAsync(userName, loginUser.Senha, loginUser.MeLembre, lockoutOnFailure: false);

    public async Task<SignInResult> PasswordSignIn(UsuarioIdentity userIdentityEntity, LoginUsuarioModel loginUser)
        => await _signInManager.PasswordSignInAsync(userIdentityEntity, loginUser.Senha, loginUser.MeLembre, lockoutOnFailure: false);

    #endregion

    #region Usuários

    public async Task<UsuarioIdentity> FindByEmail(string email)
        => await _userIdentityRepository.FindByEmailAsync(email);

    public async Task<UsuarioIdentity> GetUserIdentityById(string codigoUser)
        => await _userIdentityRepository.GetUserIdentityById(codigoUser);

    public async Task<IdentityResult> CreateUserIdentity(UsuarioIdentity entity)
    {
        var verifyEmail = await _userIdentityRepository.FindByEmailAsync(entity.Email);

        if (verifyEmail != null)
            throw new ValidationException("Conta já existente!");

        IdentityResult resultado = await _userManager.CreateAsync(entity, entity.PasswordHash);

        await _signInManager.SignInAsync(entity, false);

        return resultado;
    }

    public async Task<bool> UpdateUserIdentity(UsuarioIdentity userIdentityEntity)
    {
        var user = await _userIdentityRepository.GetUserIdentityById(userIdentityEntity.Id);

        if (user == null)
            throw new ValidationException("Não é possível encontrar o usuário.");

        if (userIdentityEntity.UserName != user.UserName)
        {
            user.UserName = userIdentityEntity.UserName;
            user.NormalizedUserName = userIdentityEntity.UserName.ToUpper();
        }

        if (userIdentityEntity.PhoneNumber != user.PhoneNumber)
        {
            user.PhoneNumber = userIdentityEntity.PhoneNumber;
        }

        await _signInManager.RefreshSignInAsync(user);

        return await _userIdentityRepository.UpdateUserIdentity(user);
    }

    public async Task<bool> InactivateUserIdentity(string codigoUser)
        => await _userIdentityRepository.InactivateUserIdentity(codigoUser);

    public async Task<bool> ReactivateUserIdentity(string codigoUser)
        => await _userIdentityRepository.ReactivateUserIdentity(codigoUser);

    public async Task<Pagination<UsuarioIdentity>> GetListByFilter(UsuarioIdentityFilter filter)
    {
        if (filter == null)
            throw new ValidationException("Filtro é nulo.");

        if (filter.PageSize > 100)
            throw new ValidationException("O tamanho máximo de página permitido é 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await _userIdentityRepository.CountByFilterAsync(filter);

        if (total == 0) return new Pagination<UsuarioIdentity>();

        var paginateResult = await _userIdentityRepository.GetListByFilterAsync(filter);

        var result = new Pagination<UsuarioIdentity>
        {
            Count = total,
            CurrentPage = filter.CurrentPage,
            PageSize = filter.PageSize,
            Result = paginateResult.ToList()
        };

        return result;
    }

    public async Task<IdentityResult> ChangePasswordUserIdentity(UsuarioIdentityMudancaSenhaEntity userIdentity)
    {
        var user = await _userManager.FindByIdAsync(userIdentity.CodigoUsuario);

        if (user == null)
            throw new ValidationException("Não é possível encontrar o usuário.");

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, userIdentity.SenhaAntiga, userIdentity.NovaSenha);

        await _signInManager.RefreshSignInAsync(user);

        return changePasswordResult;
    }

    #endregion

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion

    #region Methods Private

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, UsuarioIdentity user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string CodificarToken(ClaimsIdentity identityClaims, AuthorizationSettings authorizationSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(authorizationSettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = authorizationSettings.Issuer,
            Audience = authorizationSettings.ValidOn,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddDays(authorizationSettings.ExpirationDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //HmacSha512Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private ResponseLoginUsuarioIdentity ObterRespostaToken(string encodedToken, UsuarioIdentity user, IEnumerable<Claim> claims, AuthorizationSettings authorizationSettings)
    {
        return new ResponseLoginUsuarioIdentity
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromDays(authorizationSettings.ExpirationDays).Days,
            UserToken = new UsuarioTokenModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
       => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    #endregion
}