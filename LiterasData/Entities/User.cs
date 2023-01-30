namespace LiterasData.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Fullname { get; set; }

    public User(string login, string password, string? fullname = null)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        if (fullname != null)
        {
            Fullname = fullname;
        }
        else if (login.Contains('@'))
        {
            Fullname = login[..login.IndexOf('@')];
        }
    }
}