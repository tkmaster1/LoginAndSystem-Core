using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

public interface IMenuSistemaAppService : IDisposable
{
    Task<MenuSistemaEntity> ObterPorCodigo(int codigo);

    Task<Pagination<MenuSistemaEntity>> ObterListaPorFiltro(MenuSistemaFilter filter);

    Task<int> CriarMenuSistema(MenuSistemaEntity menuSistema);

    Task<bool> AtualizarMenuSistema(MenuSistemaEntity menuSistema);

    Task<bool> DeletarMenuSistema(int codigo);
}