using LiterasCore.Abstractions;

namespace LiterasCore.Services;

public class IdentityService : IIdentityService
{
    public Guid UserId { get; set; }
}
