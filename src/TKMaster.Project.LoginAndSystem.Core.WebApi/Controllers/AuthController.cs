using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.Request.Identity;
using TKMaster.Project.Common.Domain.Response;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Service.DTOs.UsuarioIdentity;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : MainAPIController
{
    #region Properties

    private readonly AuthorizationSettings _authorizationSettings;
    private readonly IUsuarioIdentityFacade _usuarioIdentityFacade;
    private readonly IUserAppService _user;
    public IEnumerable<string> Errors { get; set; }

    #endregion

    #region Constructor

    public AuthController(IOptions<AuthorizationSettings> authorizationSettings,
                          IUsuarioIdentityFacade usuarioIdentityFacade,
                          INotificador notificador,
                          IUserAppService user) : base(notificador, user)
    {
        _authorizationSettings = authorizationSettings.Value;
        _usuarioIdentityFacade = usuarioIdentityFacade;
        _user = user;
    }

    #endregion

    [HttpPost("entrar")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 401)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> Login(LoginUsuarioRequestDTO loginUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _usuarioIdentityFacade.Login(loginUser);

        if (result.Succeeded)
        {
            return CustomResponse(await _usuarioIdentityFacade.GerarJwt(loginUser.Email, _authorizationSettings));
        }

        if (result.IsLockedOut) return CustomResponse(loginUser);

        return CustomResponse(null, true, "Usuário ou Senha incorretos");
    }

    [HttpPost("nova-conta")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 401)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioIdentityRegisterRequestDTO registerUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _usuarioIdentityFacade.CriarUsuario(registerUser);

        if (result.Succeeded)
        {
            return CustomResponse(await _usuarioIdentityFacade.GerarJwt(registerUser.Email, _authorizationSettings));
        }

        return CustomResponse(null, true, result.Errors.Select(x => x.Description).FirstOrDefault());
    }

    [HttpGet("obterUsuarioPorCodigo")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> ObterUsuarioPorCodigo([FromQuery] string codigoUsuario)
    {
        var result = await _usuarioIdentityFacade.ObterUsuarioPorCodigo(codigoUsuario);

        return CustomResponse(result);
    }

    [HttpGet("obterUsuarioPorEmail")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> ObterUsuarioPorEmail([FromQuery] string email)
    {
        var result = await _usuarioIdentityFacade.ObterPorEmail(email);

        return CustomResponse(result);
    }
}
