using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.Common.Application.Request.Identity;
using TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.LoginAndSystem.Core.Domain.Result.UsuarioIdentity;
using TKMaster.Project.LoginAndSystem.Core.Service.DTOs.UsuarioIdentity;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;

public interface IUsuarioIdentityFacade : IDisposable
{
    #region Auth

    /// <summary>
    /// Método que realiza o Login do usuário na aplicação
    /// </summary>
    /// <param name="userRequestDto"></param>
    /// <returns></returns>
    Task<LoginUsuarioIdentityResult> Login(LoginUsuarioRequestDTO userRequestDto);

    /// <summary>
    /// Método que gera o Token para a aplicação
    /// </summary>
    /// <param name="email"></param>
    /// <param name="authorizationSettings"></param>
    /// <returns></returns>
    Task<ResponseLoginUsuarioIdentity> GerarJwt(string email, AuthorizationSettings authorizationSettings);

    /// <summary>
    /// Método que realiza o Cadastro do usuário na aplicação
    /// </summary>
    /// <param name="userIdentityEntity"></param>
    /// <returns></returns>
    Task<IdentityResult> CriarUsuario(UsuarioIdentityRegisterRequestDTO userIdentityEntity);

    ///// <summary>
    ///// Redefine a senha do usuário para o nova senha especificado após
    ///// validando a redefinição de senha fornecida com o token.
    ///// </summary>
    ///// <param name="email"></param>
    ///// <returns></returns>
    //Task<ForgotPasswordResult> EsqueceuSuaSenha(string email);

    ///// <summary>
    ///// Redefine a senha do usuário passando a nova senha especificada
    ///// validando a redefinição de senha fornecida pelo token.
    ///// </summary>
    ///// <param name="user"></param>
    ///// <param name="token"></param>UserIdentityDto user, 
    ///// <param name="novaSenha"></param>
    ///// <returns></returns>
    //Task<IdentityResult> RedefinirSenha(ResetPasswordRequestDto resetPassword);

    #endregion

    #region Usuários

    /// <summary>
    /// Método que retorna os dados do usuário através da pesquisa por Código
    /// </summary>
    /// <param name="codigoUsuario"></param>
    /// <returns></returns>
    Task<UsuarioIdentityDTO> ObterUsuarioPorCodigo(string codigoUsuario);

    /// <summary>
    /// Método que retorna os dados do usuário através da pesquisa por E-mail
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<UsuarioIdentityDTO> ObterPorEmail(string email);

    /// <summary>
    /// Método que retorna a lista de usuário através da pesquisa por vários filtros
    /// </summary>
    /// <param name="filterDto"></param>
    /// <returns></returns>
    Task<PaginationDTO<UsuarioIdentityDTO>> ListarUsuariosPorFiltros(UsuarioIdentityFilterDTO filterDto);

    /// <summary>
    /// Método de inativação de Usuário
    /// </summary>
    /// <param name="codigoUsuario"></param>
    /// <returns></returns>
    Task<bool> InativarUsuario(string codigoUsuario);

    /// <summary>
    /// Método de reativação de Usuário
    /// </summary>
    /// <param name="codigoUsuario"></param>
    /// <returns></returns>
    Task<bool> ReativarUsuario(string codigoUsuario);

    #endregion

    #region Perfil do Usuário

    /// <summary>
    /// Método que atualiza dados do perfil do usuário
    /// </summary>
    /// <param name="userIdentityEntity"></param>
    /// <returns></returns>
    Task<bool> AtualizarUsuario(UsuarioIdentityPerfilRequestDTO requestPutUsuario);

    /// <summary>
    /// Método que troca a senha do usuário
    /// </summary>
    /// <param name="changePasswordUserRequest"></param>
    /// <returns></returns>
    Task<IdentityResult> AtualizarSenhaUsuario(UsuarioIdentityMudancaSenhaRequestDTO changePasswordUserRequest);

    #endregion
}