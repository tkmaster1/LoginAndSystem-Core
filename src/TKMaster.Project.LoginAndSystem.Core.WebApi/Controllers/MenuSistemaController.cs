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
//[ClaimsAuthorize("Administracao", "Master")]
[Route("api/[controller]")]
public class MenuSistemaController : MainAPIController
{
    #region Properties

    private readonly IMenuSistemaFacade _menuSistemaFacade;
    private readonly IUserAppService _user;

    #endregion

    #region Constructor

    public MenuSistemaController(IMenuSistemaFacade menuSistemaFacade,
                                INotificador notificador,
                                IUserAppService user) : base(notificador, user)
    {
        _menuSistemaFacade = menuSistemaFacade;
        _user = user;
    }

    #endregion

    #region Methods Public

    [HttpPost("obterMenusSistema")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<ActionResult<PaginationDTO<MenuSistemaDTO>>> ObterMenusSistema([FromBody] MenuSistemaFilterDTO menuSistemaFilterDto)
    {
        var result = await _menuSistemaFacade.ListarPorFiltros(menuSistemaFilterDto);

        return CustomResponse(result);
    }

    [HttpGet("obterPorCodigo/{codigo}")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> ObterPorCodigo(int codigo)
    {
        var result = await _menuSistemaFacade.ObterPorCodigo(codigo);

        if (result == null) return NotFound();

        return CustomResponse(result);
    }

    [HttpPost("incluir")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> SalvarNovoMenuSistema([FromBody] MenuSistemaRequestDTO menuSistemaRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await _menuSistemaFacade.CriarMenuSistema(menuSistemaRequestDto);

        return CustomResponse(id);
    }

    [HttpPut("alterar")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(ResponseBaseEntity), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> AtualizarMenuSistema([FromBody] MenuSistemaRequestDTO menuSistemaRequestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return CustomResponse(await _menuSistemaFacade.AtualizarMenuSistema(menuSistemaRequestDto));
    }

    [HttpDelete("excluir/{codigo}")]
    [Consumes("application/Json")]
    [Produces("application/Json")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(ResponseFailure), 400)]
    [ProducesResponseType(typeof(ResponseFailure), 403)]
    [ProducesResponseType(typeof(ResponseFailure), 409)]
    [ProducesResponseType(typeof(ResponseFailure), 500)]
    [ProducesResponseType(typeof(ResponseFailure), 502)]
    public async Task<IActionResult> DeletarMenuSistema(int codigo)
    {
        return CustomResponse(await _menuSistemaFacade.DeletarMenuSistema(codigo));
    }

    #endregion
}