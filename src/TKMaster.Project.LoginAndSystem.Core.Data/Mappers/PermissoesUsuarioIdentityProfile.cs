using AutoMapper;
using System.Collections.Generic;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Mappers;

public class PermissoesUsuarioIdentityProfile : Profile
{
    public PermissoesUsuarioIdentityProfile()
    {
        CreateClaimsUsuarioIdentityProfile();
    }

    private void CreateClaimsUsuarioIdentityProfile()
    {
        CreateMap<PermissaoUsuarioIdentity, PermissaoUsuarioIdentityRequestDTO>().ReverseMap();
        CreateMap<PermissaoUsuarioIdentity, PermissaoUsuarioIdentityDTO>();

        CreateMap<PermissaoUsuarioIdentityFilterDTO, PermissaoUsuarioIdentityFilter>();

        CreateMap<Pagination<PermissaoUsuarioIdentity>, PaginationDTO<PermissaoUsuarioIdentityDTO>>()
            .AfterMap((source, converted, context) =>
            {
                converted.Result = context.Mapper.Map<List<PermissaoUsuarioIdentityDTO>>(source.Result);
            });
    }
}