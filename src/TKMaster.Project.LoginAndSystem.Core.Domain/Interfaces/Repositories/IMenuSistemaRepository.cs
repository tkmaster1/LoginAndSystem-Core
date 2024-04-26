using System.Collections.Generic;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

public interface IMenuSistemaRepository : IRepositoryBase<MenuSistemaEntity>
{
    Task<int> ContarPorFiltro(MenuSistemaFilter filter);

    Task<List<MenuSistemaEntity>> ObterListaPorFiltro(MenuSistemaFilter filter);
}