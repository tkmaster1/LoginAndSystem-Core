using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Application.Request.Identity;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.LoginAndSystem.Core.Domain.Models.UsuarioIdentity;
using TKMaster.Project.LoginAndSystem.Core.Service.DTOs.UsuarioIdentity;
using TKMaster.Project.LoginAndSystem.Core.Domain.Result.UsuarioIdentity;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Mappers;

public class UsuarioIdentityProfile : Profile
{
    public UsuarioIdentityProfile()
    {
        #region Login

        CreateMap<LoginUsuarioModel, LoginUsuarioRequestDTO>()
            .ReverseMap();

        CreateMap<SignInResult, LoginUsuarioIdentityResult>()
            .ReverseMap();

        #endregion

        #region Perfil e Mudança de Senha

        CreateMap<UsuarioIdentityMudancaSenhaRequestDTO, UsuarioIdentityMudancaSenhaEntity>();

        CreateMap<UsuarioIdentityDTO, UsuarioIdentityPerfilRequestDTO>()
            .ReverseMap();

        #endregion

        #region Usuario Identity

        CreateMap<UsuarioIdentityDTO, UsuarioIdentity>()
            .ForMember(o => o.Id, s => s.MapFrom(z => z.CodigoUsuario))
            .ForMember(o => o.UserName, s => s.MapFrom(z => z.Nome))
            .ForMember(o => o.NormalizedUserName, s => s.MapFrom(z => z.NomeNormalizado))
            .ForMember(o => o.PasswordHash, s => s.MapFrom(z => z.Senha))
            .ForMember(o => o.PhoneNumber, s => s.MapFrom(z => z.Telefone))
            .ForMember(o => o.NormalizedEmail, s => s.MapFrom(z => z.EmailNormalizado))
            .ForMember(o => o.EmailConfirmed, s => s.MapFrom(z => z.EmailConfirmado))
            .ReverseMap();

        CreateMap<UsuarioIdentityFilterDTO, UsuarioIdentityFilter>();

        CreateMap<Pagination<UsuarioIdentity>, PaginationDTO<UsuarioIdentityDTO>>()
            .AfterMap((source, converted, context) =>
            {
                converted.Result = context.Mapper.Map<List<UsuarioIdentityDTO>>(source.Result);
            });

        #endregion
    }
}