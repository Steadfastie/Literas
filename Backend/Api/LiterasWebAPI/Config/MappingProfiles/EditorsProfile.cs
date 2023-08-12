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

        CreateMap<EditorRequestModel, EditorDto>()
            .ForMember(
                dto => dto.Id,
                opt => opt.MapFrom(
                    mod => mod.Id))
            .ForMember(
                dto => dto.UserId,
                opt => opt.MapFrom(
                    mod => mod.UserId))
            .ForMember(
                dto => dto.DocId,
                opt => opt.MapFrom(
                    mod => mod.DocId));

        CreateMap<EditorDto, EditorResponseModel>()
            .ForMember(
                mod => mod.Id,
                opt => opt.MapFrom(
                    dto => dto.Id))
            .ForMember(
                mod => mod.UserId,
                opt => opt.MapFrom(
                    dto => dto.UserId))
            .ForMember(
                mod => mod.User,
                opt => opt.MapFrom(
                    dto => dto.User))
            .ForMember(
                mod => mod.DocId,
                opt => opt.MapFrom(
                    dto => dto.DocId))
            .ForMember(
            mod => mod.Doc,
            opt => opt.MapFrom(
                dto => dto.Doc));
    }
}