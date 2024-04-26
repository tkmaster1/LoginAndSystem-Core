using System.Collections.Generic;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

public interface IUsuarioIdentityRepository : IRepositoryBase<UsuarioIdentity>
{
    Task<UsuarioIdentity> FindByEmailAsync(string email);

    Task<UsuarioIdentity> GetUserIdentityById(string codigoUser);

    Task<int> CountByFilterAsync(UsuarioIdentityFilter filter);

    Task<List<UsuarioIdentity>> GetListByFilterAsync(UsuarioIdentityFilter filter);

    Task<bool> UpdateUserIdentity(UsuarioIdentity userIdentityEntity);

    Task<bool> InactivateUserIdentity(string codigoUser);

    Task<bool> ReactivateUserIdentity(string codigoUser);
}