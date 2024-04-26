using System.Collections.Generic;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

public interface IPermissoesUsuarioIdentityRepository : IRepositoryBase<PermissaoUsuarioIdentity>
{
    Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity);

    Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorCodigoUsuario(string codigoUsuario);

    Task<int> ContarPorFiltro(PermissaoUsuarioIdentityFilter filter);

    Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorFiltro(PermissaoUsuarioIdentityFilter filter);

    Task<bool> CriarPermissoesParaUsuarioIdentity(PermissaoUsuarioIdentity claimsUserIdentityEntity);

    Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoPermissao);
}
