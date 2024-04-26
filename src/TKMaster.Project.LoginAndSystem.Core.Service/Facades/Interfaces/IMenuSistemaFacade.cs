using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

public interface IMenuSistemaFacade : IDisposable
{
    Task<MenuSistemaDTO> ObterPorCodigo(int codigo);

    Task<PaginationDTO<MenuSistemaDTO>> ListarPorFiltros(MenuSistemaFilterDTO filterDto);

    Task<int> CriarMenuSistema(MenuSistemaRequestDTO menuSistemaRequestDto);

    Task<bool> AtualizarMenuSistema(MenuSistemaRequestDTO menuSistemaRequestDto);

    Task<bool> DeletarMenuSistema(int codigo);
}