using AutoMapper;
using LiterasData.Entities;
using LiterasDataTransfer.Dto;

namespace LiterasDataTransfer.MappingProfiles;

public class ContributorsProfile : Profile
{
    public ContributorsProfile()
    {
        CreateMap<Contributor, ContributorDto>()
            .ForMember(con => con.Id, opt => opt.MapFrom(ent => ent.Id))
            .ForMember(con => con.UserId, opt => opt.MapFrom(ent => ent.UserId))
            .ForMember(con => con.User, opt => opt.MapFrom(ent => ent.User))
            .ForMember(con => con.DocumentId, opt => opt.MapFrom(ent => ent.DocumentId))
            .ForMember(con => con.Document, opt => opt.MapFrom(ent => ent.Document));

        CreateMap<ContributorDto, Contributor>()
            .ForMember(con => con.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(con => con.UserId, opt => opt.MapFrom(dto => dto.UserId))
            .ForMember(con => con.DocumentId, opt => opt.MapFrom(dto => dto.DocumentId));
    }
}