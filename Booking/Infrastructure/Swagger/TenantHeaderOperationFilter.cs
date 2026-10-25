using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Booking.Infrastructure.Swagger;

internal sealed class TenantHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Tenant",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema { Type = JsonSchemaType.String },
            Description = "Tenant slug (dev орчинд шаардлагатай, жнь: salon1)"
        });
    }
}
