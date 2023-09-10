namespace IDT.Boss.AntifraudBlacklist.Api.Config.Swagger;

public class SwaggerOAuthSettings
{
    public string Audience { get; set; } = string.Empty;
    public string OAuthClientId { get; set; } = string.Empty;
    public string OpenIdConfigurationUrl { get; set; } = string.Empty;
    public string OAuthAuthorizationUrl { get; set; } = string.Empty;
    public string OAuthTokenUrl { get; set; } = string.Empty;
}
