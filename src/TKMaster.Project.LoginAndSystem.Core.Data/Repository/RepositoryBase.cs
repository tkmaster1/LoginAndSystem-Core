using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TKMaster.Project.LoginAndSystem.Core.Data.Context;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Repository;

public abstract class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class, new()
{
    #region Properties

    protected readonly MeuContexto DbContext;
    protected readonly IdentityContext DbIdentityContext;
    protected readonly DbSet<TEntity> DbSet;

    #endregion

    #region Constructor

    public RepositoryBase(MeuContexto meuContexto)
    {
        DbContext = meuContexto;
        DbSet = DbContext.Set<TEntity>();
    }

    public RepositoryBase(IdentityContext contextIdentity)
    {
        DbIdentityContext = contextIdentity;
        DbSet = DbIdentityContext.Set<TEntity>();
    }

    #endregion

    #region Methods Commons

    public virtual async Task<TEntity> ObterPorCodigo(int codigo)
    {
        return await DbSet.FindAsync(codigo);
    }

    public async Task<TEntity> ObterPorNome(string nome)
    {
        return await DbSet.FindAsync(nome);
    }

    public virtual async Task<bool> Existe(int codigo)
    {
        return await ObterPorCodigo(codigo) != null;
    }

    //public virtual async Task<IEnumerable<TEntity>> ObterPorCodigos(IEnumerable<int> codigos)
    //{
    //    return await DbSet.Where(x => codigos.Contains(x.Codigo)).ToListAsync();
    //}

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> ListarTodos()
    {
        return await DbSet.ToListAsync();
    }

    public IQueryable<TEntity> Obter()
    {
        return DbSet;
    }

    public virtual void Adicionar(TEntity entity)
    {
       // entity.DataCadastro = DateTime.Now;
        DbSet.Add(entity);
    }

    public virtual void Adicionar(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities.ToArray());
    }

    public virtual void Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Atualizar(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities.ToList());
    }

    public virtual void Remover(int codigo)
    {
        var entity = DbSet.Find(codigo);

        if (entity != null)
            DbSet.Remove(entity);
    }

    public virtual void Remover(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async Task<int> Salvar()
    {
        return await DbContext.SaveChangesAsync();
    }
        
    #endregion

    #region Identity

    public virtual void UpdateIdentity(TEntity entity)
    {
        var entry = DbIdentityContext.Entry(entity);

        DbSet.Attach(entity);

        entry.State = EntityState.Modified;
    }

    public async Task<int> SalvarIdentity()
    {
        return await DbIdentityContext.SaveChangesAsync();
    }

    #endregion

    public void Dispose()
    {
        if (DbContext != null)
            DbContext.Dispose();

        if (DbIdentityContext != null)
            DbIdentityContext.Dispose();

        GC.SuppressFinalize(this);
    }
}
