using Microsoft.AspNetCore.Identity;

namespace LiterasOAuth.Auth;
public class LiterasUser : IdentityUser<Guid>
{
    public LiterasUser() : base() { }
    public LiterasUser(string userName) : base(userName) { }
}
