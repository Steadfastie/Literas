using AutoMapper;
using LiterasData.Entities;
using LiterasDataTransfer.Dto;
using LiterasModels.Requests;
using LiterasModels.Responses;
using Microsoft.VisualBasic.CompilerServices;

namespace LiterasDataTransfer.MappingProfiles;

public class DocsProfile : Profile
{
    public DocsProfile()
    {
        CreateMap<Doc, DocDto>();
        CreateMap<DocDto, Doc>();

        CreateMap<DocRequestModel, DocDto>()
            .ForMember(dto => dto.Id, 
                opt => 
                    opt.MapFrom(src => Guid.NewGuid()));
        
        CreateMap<DocDto, DocResponseModel>();
        CreateMap<DocDto, DocThumbnailResponseModel>();
    }
}