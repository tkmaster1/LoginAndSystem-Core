using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.Common.Util.Configuration;
using TKMaster.Project.Common.Application.Request.Identity;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.Common.Domain.Response;
using TKMaster.Project.Common.Application.DTO.Filters;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AdminController : MainAPIController
{
    #region Properties

    private readonly AuthorizationSettings _authorizationSettings;
    private readonly IUsuarioIdentityFacade _usuarioIdentityFacade;
    private readonly IUserAppService _user;

    #endregion

    #region Constructor

    public AdminController(IUsuarioIdentityFacade usuarioIdentityFacade,
                           IOptions<AuthorizationSettings> authorizationSettings,
                           INotificador notificador,
                           IUserAppService user) : base(notificador, user)
    {
        _usuarioIdentityFacade = usuarioIdentityFacade;
        _authorizationSettings = authorizationSettings.Value;
        _user = user;
    }

    #endregion

    #region Methods Public

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

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpPost("listarUsuariosPorFiltros")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<ActionResult<PaginationDTO<UsuarioIdentityDTO>>> ListarUsuariosPorFiltros([FromBody] UsuarioIdentityFilterDTO usuarioIdentityFilterDTO)
    {
        var result = await _usuarioIdentityFacade.ListarUsuariosPorFiltros(usuarioIdentityFilterDTO);

        return CustomResponse(result);
    }

    //[HttpPost("listarUsuariosPorFiltros")]
    //[Consumes("application/Json")]
    //[Produces("application/Json")]
    //[ProducesResponseType(typeof(ResponseEntidadeBase), 200)]
    //[ProducesResponseType(typeof(ResponseFalha), 400)]
    //[ProducesResponseType(typeof(ResponseFalha), 403)]
    //[ProducesResponseType(typeof(ResponseFalha), 409)]
    //[ProducesResponseType(typeof(ResponseFalha), 500)]
    //[ProducesResponseType(typeof(ResponseFalha), 502)]
    //public async Task<IActionResult> ListarUsuariosPorFiltros([FromBody] RequestBuscarUsuarioIdentity request)
    //{
    //    var result = await _usuarioIdentityService.ListarUsuariosPorFiltros(request.ToRequest());

    //    return Response(result.Select(x => x.ToResponse()));
    //}

    [HttpPut("atualizar")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 401)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> AtualizarUsuario([FromBody] UsuarioIdentityPerfilRequestDTO requestPutUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        return CustomResponse(await _usuarioIdentityFacade.AtualizarUsuario(requestPutUser));
    }

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpPut("inativar")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> InativarUsuario([FromQuery] string codigoUsuario)
        => CustomResponse(await _usuarioIdentityFacade.InativarUsuario(codigoUsuario));

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpPut("reativar")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> ReativarUsuario([FromQuery] string codigoUsuario)
        => CustomResponse(await _usuarioIdentityFacade.ReativarUsuario(codigoUsuario));

    [HttpPost("alterarSenhaUsuario")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> AlterarSenhaUsuario([FromBody] UsuarioIdentityMudancaSenhaRequestDTO alterarSenhaUsuario)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _usuarioIdentityFacade.AtualizarSenhaUsuario(alterarSenhaUsuario);

        if (!result.Succeeded)
        {
            return CustomResponse(null, true, "Senha incorreta");
        }

        return CustomResponse(result);
    }

    #endregion
}