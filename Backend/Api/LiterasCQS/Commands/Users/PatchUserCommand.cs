using LiterasDataTransfer.Dto;
using LiterasModels.System;
using MediatR;

namespace LiterasCQS.Commands.Users;

public class PatchUserCommand : IRequest<int>
{
    public UserDto User { get; set; }
    public List<PatchModel> PatchList { get; set; }
}