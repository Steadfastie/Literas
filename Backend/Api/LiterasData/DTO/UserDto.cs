namespace LiterasData.DTO;

public class UserDto : IBaseDto
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
}