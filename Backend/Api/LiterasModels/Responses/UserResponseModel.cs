namespace LiterasModels.Responses;

public class UserResponseModel
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string? Fullname { get; set; }

    public UserResponseModel(string login, string? fullname)
    {
        Login = login;
        if (fullname != null)
        {
            Fullname = fullname;
        }
        else if (login!.Contains('@'))
        {
            Fullname = login[..login.IndexOf('@')];
        }
    }
}