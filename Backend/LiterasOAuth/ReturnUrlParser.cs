using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Diagnostics;

namespace LiterasOAuth;

public class ReturnUrlParser : IReturnUrlParser
{
    public static class ProtocolRoutePaths
    {
        public const string Authorize = "connect/authorize";
        public const string AuthorizeCallback = Authorize + "/callback";
    }

    private readonly IAuthorizeRequestValidator _validator;
    private readonly IUserSession _userSession;

    public ReturnUrlParser(
        IAuthorizeRequestValidator validator,
        IUserSession userSession)
    {
        _validator = validator;
        _userSession = userSession;
    }

    public async Task<AuthorizationRequest> ParseAsync(string returnUrl)
    {
        if (!IsValidReturnUrl(returnUrl)) return null;
        var parameters = returnUrl.ReadQueryStringAsNameValueCollection();
        var user = await _userSession.GetUserAsync();
        var result = await _validator.ValidateAsync(parameters, user);
        return !result.IsError ? result.ValidatedRequest.ToAuthorizationRequest() : null;
    }

    public bool IsValidReturnUrl(string returnUrl)
    {
        // had to add returnUrl.StartsWith("http://localhost:5000")
        // because when UI and API are not on the same host, the URL is not local
        // the condition here should be changed to either use configuration or just match domain
        if (!returnUrl.IsLocalUrl() && !returnUrl.StartsWith("https://")) return false;

        var index = returnUrl.IndexOf('?');
        if (index >= 0)
        {
            returnUrl = returnUrl.Substring(0, index);
        }

        return returnUrl.EndsWith(ProtocolRoutePaths.Authorize, StringComparison.Ordinal) ||
            returnUrl.EndsWith(ProtocolRoutePaths.AuthorizeCallback, StringComparison.Ordinal);
    }
}

internal static class Extensions
{
    public static NameValueCollection ReadQueryStringAsNameValueCollection(this string url)
    {
        var idx = url.IndexOf('?');
        if (idx >= 0)
        {
            url = url[(idx + 1)..];
        }
        var query = QueryHelpers.ParseNullableQuery(url);
        return query != null ? query.AsNameValueCollection() : new NameValueCollection();
    }

    public static bool IsLocalUrl(this string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        switch (url[0])
        {
            // Allows "/" or "/foo" but not "//" or "/\".
            // url is exactly "/"
            case '/' when url.Length == 1:
            // url doesn't start with "//" or "/\"
            case '/' when url[1] != '/' && url[1] != '\\':
                return true;
            case '/':
                return false;
            // Allows "~/" or "~/foo" but not "~//" or "~/\".
            case '~' when url.Length > 1 && url[1] == '/':
                {
                    // url is exactly "~/"
                    if (url.Length == 2)
                    {
                        return true;
                    }

                    // url doesn't start with "~//" or "~/\"
                    return url[2] != '/' && url[2] != '\\';
                }
            default:
                return false;
        }
    }

    internal static AuthorizationRequest ToAuthorizationRequest(this ValidatedAuthorizeRequest request)
    {
        var authRequest = new AuthorizationRequest(request);

        authRequest.Parameters.Add(request.Raw);

        return authRequest;
    }

    public static NameValueCollection AsNameValueCollection(this IDictionary<string, StringValues> collection)
    {
        var nv = new NameValueCollection();

        foreach (var field in collection)
        {
            nv.Add(field.Key, field.Value.First());
        }

        return nv;
    }
}
