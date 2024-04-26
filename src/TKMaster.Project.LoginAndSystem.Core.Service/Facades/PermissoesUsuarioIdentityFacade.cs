using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.Request;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.LoginAndSystem.Core.Service.Notificacoes;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Domain.Filter;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades;

public class PermissoesUsuarioIdentityFacade : BaseService, IPermissoesUsuarioIdentityFacade
{
    #region Properties

    private readonly IMapper _mapper;
    private readonly IPermissoesUsuarioIdentityAppService _permissoesUsuarioAppService;

    #endregion

    #region Constructor

    public PermissoesUsuarioIdentityFacade(IMapper mapper,
        IPermissoesUsuarioIdentityAppService permissoesUsuarioAppService,
        INotificador notificador) : base(notificador)
    {
        _mapper = mapper;
        _permissoesUsuarioAppService = permissoesUsuarioAppService;
    }

    #endregion

    #region Methods Publics

    public async Task<PaginationDTO<PermissaoUsuarioIdentityDTO>> ListarPermissoesPorFiltro(PermissaoUsuarioIdentityFilterDTO filterDto)
    {
        var filter = _mapper.Map<PermissaoUsuarioIdentityFilter>(filterDto);

        var result = await _permissoesUsuarioAppService.ObterListaPermissoesUsuarioPorFiltro(filter);

        var resultDto = _mapper.Map<PaginationDTO<PermissaoUsuarioIdentityDTO>>(result);

        return resultDto;
    }

    public async Task<PermissaoUsuarioIdentityDTO> ObterPermissaoUsuarioPorCodigo(int codigoPermissao)
    {
        var result = await _permissoesUsuarioAppService.ObterPorCodigo(codigoPermissao);

        var resultDto = _mapper.Map<PermissaoUsuarioIdentityDTO>(result);

        return resultDto;
    }

    public async Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentityRequestDTO requestDto)
    {
        var map = _mapper.Map<PermissaoUsuarioIdentity>(requestDto);

        return await _permissoesUsuarioAppService.ExistePermissaoUsuario(map);
    }

    public async Task<List<PermissaoUsuarioIdentityDTO>> ListarPermissoesUsuarioPorCodigo(string codigoUsuario)
    {
        var result = await _permissoesUsuarioAppService.ObterListaPermissoesUsuarioPorCodigoUsuario(codigoUsuario);

        var resultDto = _mapper.Map<List<PermissaoUsuarioIdentityDTO>>(result);

        return resultDto;
    }

    public async Task<bool> CriarPermissoesParaUsuario(PermissaoUsuarioIdentityRequestDTO requestDto)
    {
        var map = _mapper.Map<PermissaoUsuarioIdentity>(requestDto);

        var result = await _permissoesUsuarioAppService.CriarPermissoesParaUsuario(map);

        return result;
    }

    public async Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoPermissao)
    {
        return await _permissoesUsuarioAppService.DeletarPermissaoUsuarioPorCodigo(codigoPermissao);
    }

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion
}