using System;
using Microsoft.AspNetCore.Mvc;

namespace ODataGeneric.BaseControllers.Controllers
{
    /// <inheritdoc cref="Get"/>
    [ApiController]
    public class SwaggerJsonController : ControllerBase
    {
        /// <summary> The relative URL to download the Swagger for a Type </summary>
        public const string SwaggerJson = "/Swagger.json";
        
        /// <summary> Dynamically generates the <see cref="SwaggerJson"/> for the .NET <paramref name="assemblyQualifiedTypeName"/> </summary>
        /// <remarks>
        /// The relative URL is <see cref="SwaggerJson"/> and requires the <paramref name="assemblyQualifiedTypeName"/>:
        ///
        /// ```/Swagger.json?assemblyQualifiedName=ODataGeneric.SampleModels.Models.Counterparties.Counterparty,ODataGeneric.SampleModels```
        /// ```/Swagger.json?assemblyQualifiedName=ODataGeneric.SampleModels.Models.Counterparties.Counterparty,ODataGeneric.SampleModels```
        /// </remarks>
        [Route(SwaggerJson)]
        public IActionResult Get([FromQuery] string assemblyQualifiedName)
        {
            var type = Type.GetType(assemblyQualifiedName);
            if (type is null)
            {
                return NotFound(assemblyQualifiedName);
            }
            var baseUrl = @$"{Request.Scheme}://{Request.Host.Value}/{ODataModel.RoutePrefix}";
            return Ok(type.GenerateOpenApiDescriptionYaml(baseUrl, true));
        }
    }
}
