using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace LiterasWebAPI.Auth;

public class ClaimsTransformator : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        return Task.FromResult(NormalizeScopeClaims(principal));
    }
    
    private static ClaimsPrincipal NormalizeScopeClaims(ClaimsPrincipal principal)
    {
        var identities = new List<ClaimsIdentity>();

        foreach (var id in principal.Identities)
        {
            var identity = new ClaimsIdentity(id.AuthenticationType, id.NameClaimType, id.RoleClaimType);

            foreach (var claim in id.Claims)
            {
                if (claim.Type == "scope")
                {
                    if (claim.Value.Contains(' '))
                    {
                        var scopes = claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var scope in scopes)
                        {
                            identity.AddClaim(new Claim("scope", scope, claim.ValueType, claim.Issuer));
                        }
                    }
                    else
                    {
                        identity.AddClaim(claim);
                    }
                }
                else
                {
                    identity.AddClaim(claim);
                }
            }

            identities.Add(identity);
        }

        return new ClaimsPrincipal(identities);
    }
}
