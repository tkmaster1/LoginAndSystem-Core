using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;
using TKMaster.Project.Common.Domain.Response;
using TKMaster.Project.Common.Util.Configuration;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PermissoesSistemaController : MainAPIController
{
    #region Properties

    private readonly IPermissoesUsuarioIdentityFacade _permissoesUsuarioIdentityFacade;
    private readonly IUserAppService _user;

    #endregion

    #region Constructor

    public PermissoesSistemaController(IPermissoesUsuarioIdentityFacade permissoesUsuarioIdentityFacade,
                            INotificador notificador,
                            IUserAppService user) : base(notificador, user)
    {
        _permissoesUsuarioIdentityFacade = permissoesUsuarioIdentityFacade;
        _user = user;
    }

    #endregion

    #region Claims - Permissão Usuário

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpPost("listarPermissoesUsuarios")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<ActionResult<PaginationDTO<PermissaoUsuarioIdentityDTO>>> ListarPermissoesPorFiltros([FromBody] PermissaoUsuarioIdentityFilterDTO filterDTO)
    {
        var result = await _permissoesUsuarioIdentityFacade.ListarPermissoesPorFiltro(filterDTO);

        return CustomResponse(result);
    }

    //[HttpGet("listarPermissoesUsuarioPorCodigo")]
    //[Consumes("application/Json")]
    //[Produces("application/Json")]
    //[ProducesResponseType(typeof(ResponseEntidadeBase), 200)]
    //[ProducesResponseType(typeof(ResponseFailure), 400)]
    //[ProducesResponseType(typeof(ResponseFailure), 403)]
    //[ProducesResponseType(typeof(ResponseFailure), 409)]
    //[ProducesResponseType(typeof(ResponseFailure), 500)]
    //[ProducesResponseType(typeof(ResponseFailure), 502)]
    //public async Task<IActionResult> ListarPermissoesUsuarioPorCodigo([FromQuery] string codigoUser)
    //{
    //    var result = await _claimsUserIdentityFacade.ListarPermissoesUsuarioPorCodigo(codigoUser);

    //    return CustomResponse(result);
    //}

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpGet("obterPermissaoUsuarioPorCodigo/{codigo:int}")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> ObterPermissaoUsuarioPorCodigo(int codigo)
    {
        var result = await _permissoesUsuarioIdentityFacade.ObterPermissaoUsuarioPorCodigo(codigo);

        if (result == null) return NotFound();

        return CustomResponse(result);
    }

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpPost("incluir")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> SalvarNovoPermissaoUsuario([FromBody] PermissaoUsuarioIdentityRequestDTO requestDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _permissoesUsuarioIdentityFacade.CriarPermissoesParaUsuario(requestDto);

        return CustomResponse(result);
    }

    [ClaimsAuthorize("Administracao", "Master")]
    [HttpDelete("excluir/{codigo}")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> DeletarPermissaoUsuarioPorCodigo(int codigo)
        => CustomResponse(await _permissoesUsuarioIdentityFacade.DeletarPermissaoUsuarioPorCodigo(codigo));

    #endregion
}