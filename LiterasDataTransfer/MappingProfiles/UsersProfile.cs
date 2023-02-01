using AutoMapper;
using LiterasData.Entities;
using LiterasDataTransfer.DTO;

namespace LiterasDataTransfer.MappingProfiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(user => user.Id, opt => opt.MapFrom(ent => ent.Id))
            .ForMember(user => user.Login, opt => opt.MapFrom(ent => ent.Login))
            .ForMember(user => user.Password, opt => opt.MapFrom(ent => ent.Password))
            .ForMember(user => user.Fullname, opt => opt.MapFrom(ent => ent.Fullname));

        CreateMap<UserDTO, User>()
            .ForMember(user => user.Id, opt => opt.MapFrom(dto => dto.Id))
            .ForMember(user => user.Login, opt => opt.MapFrom(dto => dto.Login))
            .ForMember(user => user.Password, opt => opt.MapFrom(dto => dto.Password))
            .ForMember(user => user.Fullname, opt => opt.MapFrom(dto => dto.Fullname));
    }
}