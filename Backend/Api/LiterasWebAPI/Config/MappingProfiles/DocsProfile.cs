using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasWebAPI.Models.Requests;
using LiterasWebAPI.Models.Responses;

namespace LiterasWebAPI.Config.MappingProfiles;

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