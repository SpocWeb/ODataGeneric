using System;
using System.Collections.Generic;
using System.IO;
using ODataGeneric.BaseControllers.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Writers;

namespace ODataGeneric.BaseControllers
{
    public static class XOpenApiGenerator
    {
        /// <summary>Generates an OpenApi/Swagger.json Description </summary>
        public static OpenApiDocument GenerateOpenApiDescription<T>(string baseUrl) where T : class
            => GenerateOpenApiDescription(typeof(T), baseUrl);

        public static OpenApiDocument GenerateOpenApiDescription(this Type type, string baseUrl)
        {
            IEdmModel model = ODataModel.GetEntityDataModel(type);
            var settings = new OpenApiConvertSettings {
                ServiceRoot = new Uri(baseUrl)
            };
            OpenApiDocument document = model.ConvertToOpenApi(settings);

            ReplaceReferencesByCopies(document);
            
            return document;
        }

        /// <summary> Swagger cannot resolve $ref Parameter References, so we make them explicit Copies </summary>
        public static void ReplaceReferencesByCopies(OpenApiDocument document)
        {
            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var parameterList = new List<OpenApiParameter>();
                    foreach (var parameter in operation.Value.Parameters)
                    {
                        if (!string.IsNullOrEmpty(parameter.Reference?.Id))
                        {
                            if (document.Components.Parameters.TryGetValue(parameter.Reference.Id, out var parameterValue))
                            {
                                parameterList.Add(parameterValue);
                            }
                        }
                        else
                        {
                            parameterList.Add(parameter);
                        }
                    }

                    operation.Value.Parameters = parameterList;
                }
            }
        }

        public static string GenerateOpenApiDescriptionYaml<T>(string baseUrl, bool asJson) where T : class
            => GenerateOpenApiDescriptionYaml(typeof(T), baseUrl, asJson);

        public static string GenerateOpenApiDescriptionYaml(this Type type, string baseUrl, bool asJson)
        {
            OpenApiDocument openApiDocument = GenerateOpenApiDescription(type, baseUrl);

            using var ms = new MemoryStream();
            openApiDocument.Serialize(ms, OpenApiSpecVersion.OpenApi3_0, asJson ? OpenApiFormat.Json : OpenApiFormat.Yaml,
                new OpenApiWriterSettings { ReferenceInline = ReferenceInlineSetting.InlineLocalReferences });

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new(ms);
            var apiDescription = reader.ReadToEnd();
            return apiDescription.Replace("\n", "\r\n");
        }

        public const string ApiSuffix = " API";

        public static void AddSwaggerEndPoint(this SwaggerUIOptions setupAction, Type type)
        {
            setupAction.SwaggerEndpoint($@"{SwaggerJsonController.SwaggerJson
            }?{nameof(type.AssemblyQualifiedName)}={type.AssemblyQualifiedName}", type.Name + ApiSuffix);
        }
    }
}