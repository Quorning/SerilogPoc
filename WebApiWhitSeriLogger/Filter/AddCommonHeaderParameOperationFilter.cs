using System.Collections.Generic;
using Infrastructure.Trace;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiWithSeriLog.Filter
{
    public class AddCommonHeaderParameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (!(context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor) || descriptor.ControllerName.StartsWith("Home")) return;

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = TraceHeaderNames.CallSystem,
                In = ParameterLocation.Header,
                Description = "Angivelse af hvilket system der kalder.",
                Required = true,
                Schema = new OpenApiSchema() { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = TraceHeaderNames.TraceId,
                In = ParameterLocation.Header,
                Description = "Trace-Id kan angives af kalder, således at kaldet kan spores. Hvis Kalder ikke angiver et Trace-Id, sættet systemet en værdi.",
                Required = false,
                Schema = new OpenApiSchema() { Type = "uuid" }
            });


            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = TraceHeaderNames.InitielTraceId,
                In = ParameterLocation.Header,
                Description = "Initiel-Trace-Id kan angives af kalder, således at kaldet kan spores. Hvis Kalder ikke angiver et InitielTraceId, sættet systemet en værdi.",
                Required = false,
                Schema = new OpenApiSchema() { Type = "uuid" }
            });

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = TraceHeaderNames.UserId,
                In = ParameterLocation.Header,
                Description = "",
                Required = false,
                Schema = new OpenApiSchema() { Type = "string" }
            });

        }
    }
}
