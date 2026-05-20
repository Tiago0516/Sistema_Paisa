namespace SistemaPaisa.Web.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsModalRequest(this HttpRequest request) =>
        request.Query.ContainsKey("modal") ||
        string.Equals(request.Headers["X-Modal-Form"], "true", StringComparison.OrdinalIgnoreCase);
}
