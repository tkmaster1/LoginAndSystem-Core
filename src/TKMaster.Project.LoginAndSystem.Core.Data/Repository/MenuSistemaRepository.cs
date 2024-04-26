using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Repository;

public class MenuSistemaRepository : RepositoryBase<MenuSistemaEntity>, IMenuSistemaRepository
{
    #region Constructor

    public MenuSistemaRepository(IdentityContext context) : base(context)
    {
    }

    #endregion

    #region Methods

    public async Task<int> ContarPorFiltro(MenuSistemaFilter filter)
    {
        var query = DbIdentityContext.MenuSistemas.AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.CountAsync();
    }

    public async Task<List<MenuSistemaEntity>> ObterListaPorFiltro(MenuSistemaFilter filter)
    {
        var query = DbIdentityContext.MenuSistemas.AsQueryable();

        query = ApplyFilter(filter, query);

        query = ApplySorting(filter, query);

        if (filter.CurrentPage > 0)
            query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

        return await query.ToListAsync();
    }

    #endregion

    #region Methods Private

    private static IQueryable<MenuSistemaEntity> ApplyFilter(MenuSistemaFilter filter,
        IQueryable<MenuSistemaEntity> query)
    {
        if (filter.Codigo > 0)
            query = query.Where(x => x.Codigo == filter.Codigo);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(x => EF.Functions.Like(x.Nome, $"%{filter.Nome}%"));

        if (!string.IsNullOrEmpty(filter.Descricao))
            query = query.Where(x => x.Descricao.Trim().ToUpper().Contains(filter.Descricao.Trim().ToUpper()));

        if (filter.Status != int.MinValue)
        {
            if (filter.Status == 1)
                query = query.Where(x => x.Status == true);
            else if (filter.Status == 2)
                query = query.Where(x => x.Status == false);
        }

        return query;
    }

    private static IQueryable<MenuSistemaEntity> ApplySorting(MenuSistemaFilter filter,
        IQueryable<MenuSistemaEntity> query)
    {
        query = filter?.OrderBy.ToLower()
        switch
        {
            "nome" => filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.Nome)
                : query.OrderByDescending(x => x.Nome),
            _ => query.OrderBy(x => x.Nome)
        };

        return query;
    }

    #endregion
}