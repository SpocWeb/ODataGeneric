using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataGeneric.BaseControllers
{
    /// <summary> EDMX for OData </summary>
    public static class ODataModel
    {
        /// <summary> Prefix for all OData Collections, also commonly named 'odata' </summary>
        public const string RoutePrefix = "api";

        static ConstructorInfo GetConstructor(Type genericType, Type type, Type constructorParam)
        {
            TypeInfo typ = genericType.MakeGenericType(type).GetTypeInfo();
            var constructor = typ.DeclaredConstructors
                .Single(c => c.GetParameters().Last().ParameterType == constructorParam);
            return constructor;
        }

        /// <summary> Builds an EDMX from POCOs </summary>
        public static IEdmModel GetEntityDataModel<T>() where T : class => GetEntityDataModel(typeof(T));
        public static IEdmModel GetEntityDataModel(Type type)
        {
            var builder = new ODataConventionModelBuilder { Namespace = type.Name, ContainerName = type.Name + "Container" };
            //builder.AddConvention();
            builder.AddEntitySet(type);

            return builder.GetEdmModel();
        }

        public static IEdmModel GetEntityDataModel(Type[] types)
        {
            var builder = new ODataConventionModelBuilder();
            foreach (var type in types)
            {
                builder.AddEntitySet(type);
            }

            return builder.GetEdmModel();
        }

        static void AddEntitySet(this ODataConventionModelBuilder builder, Type type, bool addAcceptReject = true)
        {
            EntityTypeConfiguration entity = builder.AddEntityType(type);
            EntitySetConfiguration setConfig = builder.AddEntitySet(type.Name, entity);

            dynamic entityTypeConfiguration =
                GetConstructor(typeof(EntityTypeConfiguration<>), type, typeof(EntityTypeConfiguration)).Invoke(new object[] { builder, entity });
            dynamic entitySetConfiguration = GetConstructor(typeof(EntitySetConfiguration<>), type, typeof(EntitySetConfiguration))
                .Invoke(new object[] { builder, setConfig });

            dynamic collection = entityTypeConfiguration.Collection;
            if (!addAcceptReject)
            {
                return;
            }

            var action = collection.Action("Reject");
            AddParameters(action);
            action = collection.Action("Accept");
            AddParameters(action);
        }

        private static void AddParameters(dynamic action)
        {
            action.Returns(typeof(int));
            action.Parameter(typeof(int[]), "keys");
            action.Namespace = "Review"; //RoutePrefix + "/" + type.Name;
        }

        public static void AddRoutingFor(this ODataOptions opt, Type[] types, DefaultODataBatchHandler batchHandler)
        {
            opt.AddRouteComponents(RoutePrefix, GetEntityDataModel(types), batchHandler)
                .Select()
                .Expand()
                .OrderBy()
                .SetMaxTop(10)
                .Count()
                .Filter();
        }

        public static void AddODataRouting(this IMvcBuilder mvcBuilder, Type[] types, DefaultODataBatchHandler batchHandler)
        {
            mvcBuilder.AddOData(opt => opt.AddRoutingFor(types, batchHandler));
        }

    }
}
