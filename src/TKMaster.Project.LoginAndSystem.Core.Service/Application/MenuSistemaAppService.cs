using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Repositories;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Application;

public class MenuSistemaAppService : IMenuSistemaAppService
{
    #region Properties

    private readonly IMenuSistemaRepository _menuSistemaRepository;

    #endregion

    #region Constructor

    public MenuSistemaAppService(IMenuSistemaRepository menuSistemaRepository)
    {
        _menuSistemaRepository = menuSistemaRepository;
    }

    #endregion

    #region Methods

    public async Task<MenuSistemaEntity> ObterPorCodigo(int codigo)
        => await _menuSistemaRepository.ObterPorCodigo(codigo);

    public async Task<Pagination<MenuSistemaEntity>> ObterListaPorFiltro(MenuSistemaFilter filter)
    {
        if (filter == null)
            throw new ValidationException("Filtro é nulo.");

        if (filter.PageSize > 100)
            throw new ValidationException("O tamanho máximo de página permitido é 100.");

        if (filter.CurrentPage <= 0) filter.PageSize = 1;

        var total = await _menuSistemaRepository.ContarPorFiltro(filter);

        if (total == 0) return new Pagination<MenuSistemaEntity>();

        var paginateResult = await _menuSistemaRepository.ObterListaPorFiltro(filter);

        var result = new Pagination<MenuSistemaEntity>
        {
            Count = total,
            CurrentPage = filter.CurrentPage,
            PageSize = filter.PageSize,
            Result = paginateResult.ToList()
        };

        return result;
    }

    public async Task<int> CriarMenuSistema(MenuSistemaEntity menuSistema)
    {
        if (menuSistema == null)
            throw new ValidationException("Menu Sistema está nulo.");

        Validate(menuSistema);

        _menuSistemaRepository.Adicionar(menuSistema);

        await _menuSistemaRepository.SalvarIdentity();

        return menuSistema.Codigo;
    }

    public async Task<bool> AtualizarMenuSistema(MenuSistemaEntity menuSistema)
    {
        Validate(menuSistema, true);

        var model = await _menuSistemaRepository.ObterPorCodigo(menuSistema.Codigo);

        if (model != null)
        {
            model.Nome = menuSistema.Nome;
            model.Descricao = menuSistema.Descricao;
            model.Status = menuSistema.Status;

            _menuSistemaRepository.Atualizar(model);
        }

        return await _menuSistemaRepository.SalvarIdentity() > 0;
    }

    public async Task<bool> DeletarMenuSistema(int codigo)
    {
        var menuSistemaEntity = await _menuSistemaRepository.ObterPorCodigo(codigo);

        if (menuSistemaEntity != null)
            _menuSistemaRepository.Remover(menuSistemaEntity);

        return await _menuSistemaRepository.SalvarIdentity() > 0;
    }

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion

    #region Methods Private

    private void Validate(MenuSistemaEntity menuSistema, bool update = false)
    {
        //menuSistema.ValidarNome();

        //menuSistema.ValidarDescricao();

        //if (update)
        //    menuSistema.ValidarMenuSistemaId();
    }

    #endregion
}