using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

public interface IPermissoesUsuarioIdentityAppService : IDisposable
{
    Task<PermissaoUsuarioIdentity> ObterPorCodigo(int codigoPermissao);

    Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity);

    Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorCodigoUsuario(string codigoUsuario);

    Task<Pagination<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorFiltro(PermissaoUsuarioIdentityFilter filter);

    Task<bool> CriarPermissoesParaUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity);

    Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoPermissao);
}