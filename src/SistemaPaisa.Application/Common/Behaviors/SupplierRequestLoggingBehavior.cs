using System.Text.Json;
using MediatR;

namespace SistemaPaisa.Application.Common.Behaviors;

/// <summary>
/// Escribe en consola el request y response de los handlers de Proveedores (equivalente a console.log en servidor).
/// </summary>
public sealed class SupplierRequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (typeof(TRequest).Namespace?.Contains(".Suppliers.", StringComparison.Ordinal) != true)
            return await next();

        var requestName = typeof(TRequest).Name;
        Console.WriteLine($"[Supplier Request] {requestName}");
        Console.WriteLine(JsonSerializer.Serialize(request, JsonOptions));

        var response = await next();

        Console.WriteLine($"[Supplier Response] {requestName}");
        Console.WriteLine(JsonSerializer.Serialize(response, JsonOptions));

        return response;
    }
}
