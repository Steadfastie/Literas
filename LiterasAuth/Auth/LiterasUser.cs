using Microsoft.AspNetCore.Identity;

namespace LiterasAuth.Auth;
public class LiterasUser : IdentityUser<Guid>
{
    public LiterasUser() : base() { }
    public LiterasUser(string userName) : base(userName){}
}
