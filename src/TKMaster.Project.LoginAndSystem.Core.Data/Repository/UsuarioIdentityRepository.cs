using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Repository;

public class UsuarioIdentityRepository : RepositoryBase<UsuarioIdentity>, IUsuarioIdentityRepository
{
    #region Constructor

    public UsuarioIdentityRepository(IdentityContext context) : base(context)
    {
    }

    #endregion

    #region Methods Public

    public async Task<UsuarioIdentity> FindByEmailAsync(string email)
    {
        return await DbIdentityContext.Usuarios
                                      .AsNoTracking()
                                      .Where(x => x.Email == email)
                                      .FirstOrDefaultAsync();
    }

    public async Task<UsuarioIdentity> GetUserIdentityById(string codigoUser)
    {
        return await DbIdentityContext.Usuarios
                                      .AsNoTracking()
                                      .Where(x => x.Id == codigoUser)
                                      .FirstOrDefaultAsync();
    }

    public async Task<int> CountByFilterAsync(UsuarioIdentityFilter filter)
    {
        //if (!string.IsNullOrEmpty(filter.CodigoUsuario))
        //    return (await DbIdentityContext.Usuarios.Where(x => x.Id == filter.CodigoUsuario).ToListAsync()).Count;

        //return (await DbIdentityContext.Usuarios.ToListAsync()).Count;
        var query = DbIdentityContext.Usuarios.AsQueryable();

        query = ApplyFilter(filter, query);

        return await query.CountAsync();
    }

    public async Task<List<UsuarioIdentity>> GetListByFilterAsync(UsuarioIdentityFilter filter)
    {
        var query = DbIdentityContext.Usuarios.AsQueryable();

        //if (!string.IsNullOrEmpty(filter.CodigoUsuario))
        //    query = query.Where(x => x.Id == filter.CodigoUsuario);

        query = ApplyFilter(filter, query);

        query = ApplySorting(filter, query);

        if (filter.CurrentPage > 0)
            query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize);

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateUserIdentity(UsuarioIdentity userIdentityEntity)
    {
        DbIdentityContext.Usuarios.Update(userIdentityEntity);

        return await DbIdentityContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> InactivateUserIdentity(string codigoUser)
    {
        var user = await DbIdentityContext.Usuarios
                                           .AsNoTracking()
                                           .Where(x => x.Id == codigoUser)
                                           .FirstOrDefaultAsync();

        user.Status = false;

        DbIdentityContext.Usuarios.Update(user);

        return await DbIdentityContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> ReactivateUserIdentity(string codigoUser)
    {
        var user = await DbIdentityContext.Usuarios
                                           .AsNoTracking()
                                           .Where(x => x.Id == codigoUser)
                                           .FirstOrDefaultAsync();

        user.Status = true;

        DbIdentityContext.Usuarios.Update(user);

        return await DbIdentityContext.SaveChangesAsync() > 0;
    }

    #endregion

    #region Methods Private

    private static IQueryable<UsuarioIdentity> ApplySorting(UsuarioIdentityFilter filter,
        IQueryable<UsuarioIdentity> query)
    {
        query = filter?.OrderBy.ToLower()
            switch
        {
            "firstname" => filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.UserName)
                : query.OrderByDescending(x => x.UserName),
        };

        return query;
    }

    private static IQueryable<UsuarioIdentity> ApplyFilter(UsuarioIdentityFilter filter,
    IQueryable<UsuarioIdentity> query)
    {
        if (!string.IsNullOrEmpty(filter.CodigoUsuario))
            query = query.Where(x => x.Id == filter.CodigoUsuario);

        if (!string.IsNullOrWhiteSpace(filter.Nome))
            query = query.Where(x => x.UserName.Trim().ToUpper().Contains(filter.Nome.Trim().ToUpper()));

        if (filter.Status != int.MinValue)
        {
            if (filter.Status == 1)
                query = query.Where(x => x.Status == true);
            else if (filter.Status == 2)
                query = query.Where(x => x.Status == false);
        }

        return query;
    }

    #endregion
}