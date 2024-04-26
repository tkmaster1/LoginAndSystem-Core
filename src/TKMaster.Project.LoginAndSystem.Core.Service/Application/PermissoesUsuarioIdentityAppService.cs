using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TKMaster.Project.Common.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;
using TKMaster.Project.Common.Domain.Model;
using TKMaster.Project.Common.Domain.Filter;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Application;

public class PermissoesUsuarioIdentityAppService : IPermissoesUsuarioIdentityAppService
{
    #region Properties

    private readonly IPermissoesUsuarioIdentityRepository _permissoesUsuarioIdentityRepository;

    #endregion

    #region Constructor

    public PermissoesUsuarioIdentityAppService(IPermissoesUsuarioIdentityRepository permissoesUsuarioIdentityRepository)
        => _permissoesUsuarioIdentityRepository = permissoesUsuarioIdentityRepository;

    #endregion

    #region Methods Publics

    public async Task<PermissaoUsuarioIdentity> ObterPorCodigo(int codigoPermissao)
    {
        if (codigoPermissao <= 0)
            throw new ValidationException("Código obrigatório.");

        return await _permissoesUsuarioIdentityRepository.ObterPorCodigo(codigoPermissao);
    }

    public async Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity)
        => await _permissoesUsuarioIdentityRepository.ExistePermissaoUsuario(permissaoUsuarioIdentity);

    public async Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorCodigoUsuario(string codigoUsuario)
    {
        if (string.IsNullOrEmpty(codigoUsuario))
            throw new ValidationException("Código do Usuário obrigatório.");

        return await _permissoesUsuarioIdentityRepository.ObterListaPermissoesUsuarioPorCodigoUsuario(codigoUsuario);
    }

    public async Task<Pagination<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorFiltro(PermissaoUsuarioIdentityFilter filter)
    {
        if (filter == null)
            throw new ValidationException("Filtro é nulo.");

        if (filter.PageSize > 100)
            throw new ValidationException("O tamanho máximo de página permitido é 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await _permissoesUsuarioIdentityRepository.ContarPorFiltro(filter);

        if (total == 0) return new Pagination<PermissaoUsuarioIdentity>();

        var paginateResult = await _permissoesUsuarioIdentityRepository.ObterListaPermissoesUsuarioPorFiltro(filter);

        var result = new Pagination<PermissaoUsuarioIdentity>
        {
            Count = total,
            CurrentPage = filter.CurrentPage,
            PageSize = filter.PageSize,
            Result = paginateResult.ToList()
        };

        return result;
    }

    public async Task<bool> CriarPermissoesParaUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity)
        => await _permissoesUsuarioIdentityRepository.CriarPermissoesParaUsuarioIdentity(permissaoUsuarioIdentity);

    public async Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoPermissao)
    {
        if (codigoPermissao <= 0)
            throw new ValidationException("Código obrigatório.");

        return await _permissoesUsuarioIdentityRepository.DeletarPermissaoUsuarioPorCodigo(codigoPermissao);
    }

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion
}