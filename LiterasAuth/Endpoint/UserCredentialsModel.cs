using System.ComponentModel.DataAnnotations;

namespace LiterasAuth.Endpoint;
public class UserCredentialsModel
{
    public string Username { get; set; }
    public string Password { get; set; }

    //[StringLength(int.MaxValue, MinimumLength = 1)]
    public string ReturnUrl { get; set; }
}
