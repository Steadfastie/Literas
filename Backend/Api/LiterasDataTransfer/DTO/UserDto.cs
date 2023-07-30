using LiterasModels.Abstractions;

namespace LiterasDataTransfer.Dto;

public class UserDto : IBaseDto
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
}