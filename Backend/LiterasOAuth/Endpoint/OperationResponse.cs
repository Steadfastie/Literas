namespace LiterasOAuth.Endpoint;
public class OperationResponse
{
    public string Type { get; set; }
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ReturnUrl { get; set; }
}
