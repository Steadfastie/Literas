using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasWebAPI.Models.Requests;
using LiterasWebAPI.Models.Responses;

namespace LiterasWebAPI.Config.MappingProfiles;

public class EditorsProfile : Profile
{
    public EditorsProfile()
    {
        CreateMap<Editor, EditorDto>();
        CreateMap<EditorDto, Editor>();

        CreateMap<EditorRequestModel, EditorDto>();
        CreateMap<EditorDto, EditorResponse>();
    }
}
