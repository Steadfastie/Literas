using AutoMapper;
using LiterasData.Entities;
using LiterasDataTransfer.Dto;
using LiterasModels.Requests;
using LiterasModels.Responses;

namespace LiterasDataTransfer.MappingProfiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(user => user.Id, opt => opt.MapFrom(ent => ent.Id))
            .ForMember(user => user.Login, opt => opt.MapFrom(ent => ent.Login))
            .ForMember(user => user.Password, opt => opt.MapFrom(ent => ent.Password))
            .ForMember(user => user.Fullname, opt => opt.MapFrom(ent => ent.Fullname));

        CreateMap<UserDto, User>()
            .ForCtorParam("login", opt => opt.MapFrom(dto => dto.Login))
            .ForCtorParam("password", opt => opt.MapFrom(dto => dto.Password))
            .ForCtorParam("fullname", opt => opt.MapFrom(dto => dto.Fullname))
            .ForMember(user => user.Id, opt => opt.MapFrom(dto => dto.Id));

        CreateMap<UserDto, UserResponseModel>()
           .ForCtorParam("login", opt => opt.MapFrom(dto => dto.Login))
           .ForCtorParam("fullname", opt => opt.MapFrom(dto => dto.Fullname));

        CreateMap<UserRequestModel, UserDto>()
           .ForMember(dto => dto.Login, opt => opt.MapFrom(model => model.Login))
           .ForMember(dto => dto.Password, opt => opt.MapFrom(model => model.Password))
           .ForMember(dto => dto.Fullname, opt => opt.MapFrom(model => model.Fullname));
    }
}