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
        CreateMap<Doc, DocDto>()
            .ForMember(doc => doc.Id, opt => opt.MapFrom(ent => ent.Id))
            .ForMember(doc => doc.CreatorId, opt => opt.MapFrom(ent => ent.CreatorId))
            .ForMember(doc => doc.Title, opt => opt.MapFrom(ent => ent.Title))
            .ForMember(doc => doc.Content, opt => opt.MapFrom(ent => ent.Content));

        CreateMap<DocDto, Doc>()
            .ForMember(doc => doc.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(doc => doc.CreatorId, opt => opt.MapFrom(dto => dto.CreatorId))
            .ForMember(doc => doc.Title, opt => opt.MapFrom(dto => dto.Title))
            .ForMember(doc => doc.Content, opt => opt.MapFrom(dto => dto.Content));

        CreateMap<DocRequestModel, DocDto>()
            .ForMember(
                dto => dto.Id,
                opt => opt.MapFrom(
                    mod => mod.Id))
            .ForMember(
                dto => dto.Title,
                opt => opt.MapFrom(
                    mod => mod.Title))
            .ForMember(
            dto => dto.Content,
            opt => opt.MapFrom(
                mod => mod.Content));
        
        CreateMap<DocDto, DocResponseModel>()
            .ForMember(
                mod => mod.Id,
                opt => opt.MapFrom(
                    dto => dto.Id))
            .ForMember(
                mod => mod.CreatorId,
                opt => opt.MapFrom(
                    dto => dto.CreatorId))
            .ForMember(
                mod => mod.Title,
                opt => opt.MapFrom(
                    dto => dto.Title))
            .ForMember(
                mod => mod.Content,
                opt => opt.MapFrom(
                    dto => dto.Content));

        CreateMap<DocDto, DocThumbnailResponseModel>()
            .ForMember(
                mod => mod.Id,
                opt => opt.MapFrom(
                    dto => dto.Id))
            .ForMember(
                mod => mod.Title,
                opt => opt.MapFrom(
                    dto => dto.Title));
    }
}