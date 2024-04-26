using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Util.Common;
using TKMaster.Project.Common.Application.Request.Identity;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Services;
using TKMaster.Project.LoginAndSystem.Core.Service.Facades.Interfaces;
using TKMaster.Project.LoginAndSystem.Core.Service.Notificacoes;
using TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;
using TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.LoginAndSystem.Core.Domain.Result.UsuarioIdentity;
using TKMaster.Project.LoginAndSystem.Core.Service.DTOs.UsuarioIdentity;

namespace TKMaster.Project.LoginAndSystem.Core.Service.Facades;

public class UsuarioIdentityFacade : BaseService, IUsuarioIdentityFacade
{
    #region Properties

    private readonly IMapper _mapper;
    private readonly IUsuarioIdentityAppService _usuarioIdentityAppService;

    #endregion

    #region Constructor

    public UsuarioIdentityFacade(IMapper mapper,
        IUsuarioIdentityAppService usuarioIdentityAppService,
        INotificador notificador) : base(notificador)
    {
        _mapper = mapper;
        _usuarioIdentityAppService = usuarioIdentityAppService;
    }

    #endregion

    #region Methods Publics

    #region Login

    public async Task<LoginUsuarioIdentityResult> Login(LoginUsuarioRequestDTO userRequestDto)
    {
        var user = await _usuarioIdentityAppService.FindByEmail(userRequestDto.Email);

        if (user == null) throw new Exception("Usuário ou Senha incorretos");

        var login = _mapper.Map<LoginUsuarioModel>(userRequestDto);

        var result = await _usuarioIdentityAppService.PasswordSignIn(user.UserName, login);

        return _mapper.Map<LoginUsuarioIdentityResult>(result);
    }

    public async Task<ResponseLoginUsuarioIdentity> GerarJwt(string email, AuthorizationSettings authorizationSettings)
    {
        return await _usuarioIdentityAppService.GerarJwt(email, authorizationSettings);
    }

    #endregion

    #region Usuários

    public async Task<UsuarioIdentityDTO> ObterUsuarioPorCodigo(string codigoUsuario)
    {
        var result = await _usuarioIdentityAppService.GetUserIdentityById(codigoUsuario);

        return _mapper.Map<UsuarioIdentityDTO>(result);
    }

    public async Task<UsuarioIdentityDTO> ObterPorEmail(string email)
    {
        var result = await _usuarioIdentityAppService.FindByEmail(email);

        return _mapper.Map<UsuarioIdentityDTO>(result);
    }

    public async Task<PaginationDTO<UsuarioIdentityDTO>> ListarUsuariosPorFiltros(UsuarioIdentityFilterDTO filterDto)
    {
        var filter = _mapper.Map<UsuarioIdentityFilter>(filterDto);

        var result = await _usuarioIdentityAppService.GetListByFilter(filter);

        return _mapper.Map<PaginationDTO<UsuarioIdentityDTO>>(result);
    }

    public async Task<IdentityResult> CriarUsuario(UsuarioIdentityRegisterRequestDTO userIdentityRequestDto)
    {
        var user = GetUserFromRegisterUser(userIdentityRequestDto);

        var usuario = _mapper.Map<UsuarioIdentity>(user);

        return await _usuarioIdentityAppService.CreateUserIdentity(usuario);
    }

    public async Task<bool> InativarUsuario(string codigoUsuario)
       => await _usuarioIdentityAppService.InactivateUserIdentity(codigoUsuario);

    public async Task<bool> ReativarUsuario(string codigoUsuario)
        => await _usuarioIdentityAppService.ReactivateUserIdentity(codigoUsuario);

    #endregion

    #region Perfil do Usuário

    /// <summary>
    /// Atualização do Perfil do Usuário
    /// </summary>
    /// <param name="userIdentityDto"></param>
    /// <returns></returns>
    public async Task<bool> AtualizarUsuario(UsuarioIdentityPerfilRequestDTO userIdentityDto)
    {
        var user = GetUserFromUpdateUser(userIdentityDto);

        var userIdentityEntity = _mapper.Map<UsuarioIdentity>(user);

        return await _usuarioIdentityAppService.UpdateUserIdentity(userIdentityEntity);
    }

    /// <summary>
    /// Atualização da senha do usuário na parte de perfil do usuário
    /// </summary>
    /// <param name="changePasswordUserRequest"></param>
    /// <returns></returns>
    public async Task<IdentityResult> AtualizarSenhaUsuario(UsuarioIdentityMudancaSenhaRequestDTO changePasswordUserRequest)
    {
        var usuario = _mapper.Map<UsuarioIdentityMudancaSenhaEntity>(changePasswordUserRequest);

        return await _usuarioIdentityAppService.ChangePasswordUserIdentity(usuario);
    }

    #endregion

    public void Dispose() => GC.SuppressFinalize(this);

    #endregion

    #region Methods Private

    private UsuarioIdentityDTO GetUserFromRegisterUser(UsuarioIdentityRegisterRequestDTO registerUser)
    {
        return new UsuarioIdentityDTO
        {
            CodigoUsuario = Guid.NewGuid().ToString(),
            Nome = registerUser.Email.Substring(0, registerUser.Email.IndexOf("@")),
            NomeNormalizado = registerUser.Email.Substring(0, registerUser.Email.IndexOf("@")).ToUpper(),
            Senha = registerUser.Senha,
            Email = registerUser.Email,
            EmailNormalizado = registerUser.Email.ToUpper(),
            Status = true,
            EmailConfirmado = true
        };
    }

    private UsuarioIdentityDTO GetUserFromUpdateUser(UsuarioIdentityPerfilRequestDTO requestPutDto)
    {
        return new UsuarioIdentityDTO
        {
            CodigoUsuario = requestPutDto.CodigoUsuario,
            Nome = requestPutDto.Nome,
            NomeNormalizado = requestPutDto.Nome.ToUpper(),
            Telefone = requestPutDto.Telefone,
            Status = true
        };
    }

    #endregion
}