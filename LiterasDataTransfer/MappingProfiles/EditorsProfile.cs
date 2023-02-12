using AutoMapper;
using LiterasData.Entities;
using LiterasDataTransfer.Dto;

namespace LiterasDataTransfer.MappingProfiles;

public class EditorsProfile : Profile
{
    public EditorsProfile()
    {
        CreateMap<Editor, EditorDto>()
            .ForMember(con => con.Id, opt => opt.MapFrom(ent => ent.Id))
            .ForMember(con => con.UserId, opt => opt.MapFrom(ent => ent.UserId))
            .ForMember(con => con.User, opt => opt.MapFrom(ent => ent.User))
            .ForMember(con => con.DocId, opt => opt.MapFrom(ent => ent.DocId))
            .ForMember(con => con.Doc, opt => opt.MapFrom(ent => ent.Doc));

        CreateMap<EditorDto, Editor>()
            .ForMember(con => con.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(con => con.UserId, opt => opt.MapFrom(dto => dto.UserId))
            .ForMember(con => con.DocId, opt => opt.MapFrom(dto => dto.DocId));
    }
}