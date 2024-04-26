using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{
    Task<TEntity> ObterPorCodigo(int codigo);

    Task<TEntity> ObterPorNome(string nome);

    Task<bool> Existe(int codigo);

   // Task<IEnumerable<TEntity>> ObterPorCodigos(IEnumerable<int> codigos);

    Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> ListarTodos();

    IQueryable<TEntity> Obter();

    void Adicionar(TEntity entity);

    void Adicionar(IEnumerable<TEntity> entities);

    void Atualizar(TEntity entity);

    void Atualizar(IEnumerable<TEntity> entities);

    void Remover(int codigo);

    void Remover(TEntity entity);

    Task<int> Salvar();

    #region Identity

    void UpdateIdentity(TEntity entity);

    Task<int> SalvarIdentity();

    #endregion
}
