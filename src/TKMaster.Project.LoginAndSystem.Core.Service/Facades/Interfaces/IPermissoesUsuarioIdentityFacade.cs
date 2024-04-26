using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

public interface IPermissoesUsuarioIdentityFacade : IDisposable
{
    /// <summary>
    /// Listar todas as permissões por Filtro
    /// </summary>
    /// <param name="filterDto"></param>
    /// <returns></returns>
    Task<PaginationDTO<PermissaoUsuarioIdentityDTO>> ListarPermissoesPorFiltro(PermissaoUsuarioIdentityFilterDTO filterDto); // GetListClaimsByFilterAsync

    /// <summary>
    /// Obter permissão do Usuário por Codigo Claim
    /// </summary>
    /// <param name="codigoClaim"></param>
    /// <returns></returns>
    Task<PermissaoUsuarioIdentityDTO> ObterPermissaoUsuarioPorCodigo(int codigoClaim); // GetClaimUserIdentityById

    /// <summary>
    /// Verifica se o usuário possui as mesmas permissões que está tentando incluir.
    /// para evitar duplicidade
    /// </summary>
    /// <param name="permissaoUsuarioIdentity"></param>
    /// <returns></returns>
    Task<bool> ExistePermissaoUsuario(PermissaoUsuarioIdentityRequestDTO requestDto);

    /// <summary>
    /// Listar Permissões de Usuario por Codigo do Usuário
    /// </summary>
    /// <param name="codigoUser"></param>
    /// <returns></returns>
    Task<List<PermissaoUsuarioIdentityDTO>> ListarPermissoesUsuarioPorCodigo(string codigoUser); // GetClaimsUserIdentityByUserId

    /// <summary>
    /// Criar permissões para o usuário
    /// </summary>
    /// <param name="requestDto"></param>
    /// <returns></returns>
    Task<bool> CriarPermissoesParaUsuario(PermissaoUsuarioIdentityRequestDTO requestDto); // CreateClaimsForUserIdentity

    /// <summary>
    /// Excluir uma permissão por codigo Claim
    /// </summary>
    /// <param name="codigoClaim"></param>
    Task<bool> DeletarPermissaoUsuarioPorCodigo(int codigoClaim); // DeleteClaimsByUserId
}