using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Filters;

using DNSeed.Security;

namespace DNSeed.Filters
{
    internal sealed class ApiKeyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filter = new SecurityRequirementsOperationFilter(securitySchemaName: ApiKeyAuthenticationOptions.DefaultScheme);
            filter.Apply(operation, context);
        }
    }
}
