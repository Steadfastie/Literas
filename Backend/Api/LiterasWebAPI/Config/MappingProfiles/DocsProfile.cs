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
        CreateMap<DocRequestModel, DocDto>();

        CreateMap<Doc, DocDto>();
    }
}
