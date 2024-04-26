using AutoMapper;
using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades;

public class MenuSistemaFacade : IMenuSistemaFacade
{
    #region Properties

    private readonly IMapper _mapper;
    private readonly IMenuSistemaAppService _menuSistemaService;

    #endregion

    #region Constructor

    public MenuSistemaFacade(IMapper mapper, IMenuSistemaAppService menuSistemaService)
    {
        _mapper = mapper;
        _menuSistemaService = menuSistemaService;
    }

    #endregion

    #region Methods

    public async Task<MenuSistemaDTO> ObterPorCodigo(int codigo)
    {
        var modalidade = await _menuSistemaService.ObterPorCodigo(codigo);

        return _mapper.Map<MenuSistemaDTO>(modalidade);
    }

    public async Task<PaginationDTO<MenuSistemaDTO>> ListarPorFiltros(MenuSistemaFilterDTO filterDto)
    {
        var filter = _mapper.Map<MenuSistemaFilter>(filterDto);

        var result = await _menuSistemaService.ObterListaPorFiltro(filter);

        var resultDto = _mapper.Map<PaginationDTO<MenuSistemaDTO>>(result);

        return resultDto;
    }

    public async Task<int> CriarMenuSistema(MenuSistemaRequestDTO menuSistemaRequestDto)
    {
        var menuSistemaDomain = _mapper.Map<MenuSistemaEntity>(menuSistemaRequestDto);

        int codigo = await _menuSistemaService.CriarMenuSistema(menuSistemaDomain);

        return codigo;
    }

    public async Task<bool> AtualizarMenuSistema(MenuSistemaRequestDTO menuSistemaRequestDto)
    {
        var menuSistemaDomain = _mapper.Map<MenuSistemaEntity>(menuSistemaRequestDto);

        return await _menuSistemaService.AtualizarMenuSistema(menuSistemaDomain);
    }

    public async Task<bool> DeletarMenuSistema(int codigo) =>
        await _menuSistemaService.DeletarMenuSistema(codigo);

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion
}