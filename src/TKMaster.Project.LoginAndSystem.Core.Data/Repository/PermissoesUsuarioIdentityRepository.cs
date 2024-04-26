using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Repository;

public class PermissoesUsuarioIdentityRepository : RepositoryBase<PermissaoUsuarioIdentity>, IPermissoesUsuarioIdentityRepository
{
    #region Constructor

    public PermissoesUsuarioIdentityRepository(IdentityContext context) : base(context)
    {
    }

    #endregion

    #region Methods

    public async Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorCodigoUsuario(string codigoUsuario)
    {
        var query = await DbIdentityContext.PermissoesUsuarios
                                           .Where(x => x.UserId == codigoUsuario)
                                           .ToListAsync();

        var list = new List<PermissaoUsuarioIdentity>();

        if (query.Count > 0)
        {
            list.AddRange(from item in query
                          select new PermissaoUsuarioIdentity()
                          {
                              CodigoUsuario = item.UserId,
                              Menu = item.ClaimType,
                              Perfil = item.ClaimValue,
                              CodigoClaim = item.Id
                          });
        }

        return list.ToList();
    }

    public async override Task<PermissaoUsuarioIdentity> ObterPorCodigo(int codigoPermissao)
    {
        var query = await DbIdentityContext.PermissoesUsuarios
                                          .Where(x => x.Id == codigoPermissao)
                                          .FirstOrDefaultAsync();

        var query1 = await DbIdentityContext.MenuSistemas
                                          .Where(x => x.Nome == query.ClaimType)
                                          .ToListAsync();

        var objClaim = new PermissaoUsuarioIdentity();

        if (query == null)
            return objClaim;

        switch (query1.Count)
        {
            case > 0:
                {
                    foreach (var item in query1)
                    {
                        objClaim = new PermissaoUsuarioIdentity()
                        {
                            CodigoClaim = query.Id,
                            CodigoUsuario = query.UserId,
                            Menu = query.ClaimType,
                            Perfil = query.ClaimValue,
                            MenuNormalizado = item.Descricao
                        };
                    }

                    break;
                }

            default:
                objClaim = new PermissaoUsuarioIdentity()
                {
                    CodigoClaim = query.Id,
                    CodigoUsuario = query.UserId,
                    Menu = query.ClaimType,
                    Perfil = query.ClaimValue
                };
                break;
        }

        return objClaim;
    }

    public async Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentity permissaoUsuarioIdentity)
    {
        var query = await DbIdentityContext.PermissoesUsuarios
                                          .Where(x => x.UserId == permissaoUsuarioIdentity.CodigoUsuario
                                          && x.ClaimType == permissaoUsuarioIdentity.Menu
                                          && x.ClaimValue == permissaoUsuarioIdentity.Perfil)
                                          .FirstOrDefaultAsync();

        if (query == null) return false;

        return true;
    }

    public async Task<int> ContarPorFiltro(PermissaoUsuarioIdentityFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.CodigoUsuario))
            return (await DbIdentityContext.PermissoesUsuarios
                                           .Where(x => x.UserId == filter.CodigoUsuario)
                                           .ToListAsync()).Count;

        return (await DbIdentityContext.PermissoesUsuarios.ToListAsync()).Count;
    }

    public async Task<List<PermissaoUsuarioIdentity>> ObterListaPermissoesUsuarioPorFiltro(PermissaoUsuarioIdentityFilter filter)
    {
        var query = new List<IdentityUserClaim<string>>();

        if (!string.IsNullOrEmpty(filter.CodigoUsuario))
            query = await DbIdentityContext.PermissoesUsuarios.Where(x => x.UserId == filter.CodigoUsuario).ToListAsync();
        else
            query = await DbIdentityContext.PermissoesUsuarios.ToListAsync();

        if (!string.IsNullOrWhiteSpace(filter.Menu))
            query = query.Where(x => x.ClaimType.Trim().ToUpper().Contains(filter.Menu.Trim().ToUpper())).ToList();

        if (filter.CurrentPage > 0)
            query = query.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize).ToList();

        var list = new List<PermissaoUsuarioIdentity>();

        if (query.Count > 0)
        {
            var query1 = await DbIdentityContext.MenuSistemas.Where(x => x.Status).ToListAsync();

            list.AddRange(from item in query
                          join m in query1 on item.ClaimType equals m.Nome into leftJ
                          from lj in leftJ.DefaultIfEmpty()
                          select new PermissaoUsuarioIdentity()
                          {
                              CodigoUsuario = item.UserId,
                              Menu = item.ClaimType,
                              Perfil = item.ClaimValue,
                              CodigoClaim = item.Id,
                              MenuNormalizado = lj != null ? lj.Descricao : string.Empty,
                          });
        }

        return list.ToList();
    }

    public async Task<bool> CriarPermissoesParaUsuarioIdentity(PermissaoUsuarioIdentity permissaoUsuarioIdentity)
    {
        try
        {
            var query = await DbIdentityContext.PermissoesUsuarios.Where(x => x.UserId == permissaoUsuarioIdentity.CodigoUsuario).ToListAsync();

            var claimsUser = permissaoUsuarioIdentity.Perfil.Split(',')
                                                     .Select(perfil => new Claim(permissaoUsuarioIdentity.Menu, perfil))
                                                     .ToList();

            if (query.Count > 0)
            {
                foreach (var item in claimsUser)
                {
                    var existeClaims = await DbIdentityContext.PermissoesUsuarios
                                          .Where(x => x.UserId == permissaoUsuarioIdentity.CodigoUsuario
                                          && x.ClaimType == item.Type
                                          && x.ClaimValue == item.Value)
                                          .FirstOrDefaultAsync();

                    if (existeClaims == null)
                    {
                        var listClaims = (from clm in claimsUser
                                          from it in query
                                          where clm.Type == it.ClaimType && clm.Value != it.ClaimValue
                                          let f = new IdentityUserClaim<string>()
                                          {
                                              ClaimType = clm.Type,
                                              ClaimValue = clm.Value,
                                              UserId = permissaoUsuarioIdentity.CodigoUsuario
                                          }
                                          select f).ToList();

                        if (!listClaims.Any())
                        {
                            listClaims = (from clm in claimsUser
                                          let f = new IdentityUserClaim<string>()
                                          {
                                              ClaimType = clm.Type,
                                              ClaimValue = clm.Value,
                                              UserId = permissaoUsuarioIdentity.CodigoUsuario
                                          }
                                          select f).ToList();
                        }

                        DbIdentityContext.PermissoesUsuarios.AddRange(listClaims);
                    }
                }

                //var listClaims = (from item in claimsUser
                //                  from it in query
                //                  where item.Type == it.ClaimType && item.Value != it.ClaimValue
                //                  let f = new IdentityUserClaim<string>()
                //                  {
                //                      ClaimType = item.Type,
                //                      ClaimValue = item.Value,
                //                      UserId = permissaoUsuarioIdentity.CodigoUser
                //                  }
                //                  select f).ToList();

                //if (!listClaims.Any())
                //{
                //    listClaims = (from item in claimsUser
                //                  let f = new IdentityUserClaim<string>()
                //                  {
                //                      ClaimType = item.Type,
                //                      ClaimValue = item.Value,
                //                      UserId = permissaoUsuarioIdentity.CodigoUser
                //                  }
                //                  select f).ToList();
                //}

                //DbIdentityContext.PermissoesUsuarios.AddRange(listClaims);
            }
            else
            {
                var param = (from item in claimsUser
                             let f = new IdentityUserClaim<string>()
                             {
                                 ClaimType = item.Type,
                                 ClaimValue = item.Value,
                                 UserId = permissaoUsuarioIdentity.CodigoUsuario
                             }
                             select f).ToList();

                DbIdentityContext.PermissoesUsuarios.AddRange(param);
            }

            DbIdentityContext.SaveChanges();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoPermissao)
    {
        var query = DbIdentityContext.PermissoesUsuarios
                                     .Where(x => x.Id == codigoPermissao)
                                     .FirstOrDefault();

        DbIdentityContext.PermissoesUsuarios.RemoveRange(query);

        return await DbIdentityContext.SaveChangesAsync() > 0;
    }

    #endregion

    #region Methods Private

    //private static IQueryable<PermissaoUsuarioIdentity> ApplyFilter(PermissaoUsuarioIdentityFilter filter,
    //    IQueryable<PermissaoUsuarioIdentity> query)
    //{
    //    if (!string.IsNullOrEmpty(filter.CodigoUsuario))
    //        query = query.Where(x => x.CodigoUsuario == filter.CodigoUsuario);

    //    //if (!string.IsNullOrWhiteSpace(filter.Menu))
    //    //    query = query.Where(x => EF.Functions.Like(x.Menu, $"%{filter.Menu}%"));

    //    //if (!string.IsNullOrEmpty(filter.Descricao))
    //    //    query = query.Where(x => x.Descricao.Trim().ToUpper().Contains(filter.Descricao.Trim().ToUpper()));

    //    //if (filter.Status != int.MinValue)
    //    //{
    //    //    if (filter.Status == 1)
    //    //        query = query.Where(x => x.Status == true);
    //    //    else if (filter.Status == 2)
    //    //        query = query.Where(x => x.Status == false);
    //    //}

    //    return query;
    //}

    private static IQueryable<PermissaoUsuarioIdentity> ApplySorting(PermissaoUsuarioIdentityFilter filter,
        IQueryable<PermissaoUsuarioIdentity> query)
    {
        query = filter?.OrderBy.ToLower()
        switch
        {
            "firstname" => filter.SortBy.ToLower() == "asc"
                ? query.OrderBy(x => x.Menu)
                : query.OrderByDescending(x => x.Menu),
            _ => query.OrderBy(x => x.Menu)
        };

        return query;
    }

    #endregion
}