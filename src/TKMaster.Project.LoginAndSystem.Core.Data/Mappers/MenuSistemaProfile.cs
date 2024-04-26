using AutoMapper;
using System.Collections.Generic;
using TKMaster.Project.Common.Application.DTO;
using TKMaster.Project.Common.Application.DTO.Filters;
using TKMaster.Project.Common.Application.Request;
using TKMaster.Project.Common.Domain.Entities;
using TKMaster.Project.Common.Domain.Filter;
using TKMaster.Project.Common.Domain.Model;

namespace TKMaster.Project.LoginAndSystem.Core.Data.Mappers;

public class MenuSistemaProfile : Profile
{
    public MenuSistemaProfile()
    {
        CreateMap<MenuSistemaEntity, MenuSistemaDTO>().ReverseMap();
        CreateMap<MenuSistemaFilterDTO, MenuSistemaFilter>();
        CreateMap<MenuSistemaRequestDTO, MenuSistemaEntity>();

        CreateMap<Pagination<MenuSistemaEntity>, PaginationDTO<MenuSistemaDTO>>()
        .AfterMap((source, converted, context) =>
        {
            converted.Result = context.Mapper.Map<List<MenuSistemaDTO>>(source.Result);
        });
    }
}