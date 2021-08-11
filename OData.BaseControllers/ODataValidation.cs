using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;

namespace ODataGeneric.BaseControllers
{
    public static class ODataValidation
    {
        public const int MaxPageSize = 99;

        /// <summary> Global Validation Settings </summary>
        public static readonly ODataValidationSettings? Settings = new()
        {
            MaxNodeCount = MaxPageSize,
            MaxExpansionDepth = 3,
            MaxSkip = 10,
            MaxTop = 5,
            AllowedFunctions = AllowedFunctions.AllMathFunctions,
        };

        public static ODataValidationSettings AsValidationSettings(this EnableQueryAttribute attribute)
        {
            var ret = new ODataValidationSettings
            {
                AllowedArithmeticOperators = attribute.AllowedArithmeticOperators,
                AllowedLogicalOperators = attribute.AllowedLogicalOperators,
                AllowedFunctions = attribute.AllowedFunctions,
                AllowedQueryOptions = attribute.AllowedQueryOptions,
                MaxAnyAllExpressionDepth = attribute.MaxAnyAllExpressionDepth,
                MaxExpansionDepth = attribute.MaxExpansionDepth,
                MaxNodeCount = attribute.MaxNodeCount,
                MaxOrderByNodeCount = attribute.MaxOrderByNodeCount,
                MaxSkip = attribute.MaxSkip,
                MaxTop = attribute.MaxTop
            };
            foreach (var orderBy in attribute.AllowedOrderByProperties.Split(','))
            {
                ret.AllowedOrderByProperties.Add(orderBy);
            }
            return ret;
        }

        public static EnableQueryAttribute AsValidationSettings(this ODataValidationSettings settings
        , int maxSkip = 999, int maxTop = MaxPageSize, int pageSize = MaxPageSize
        , HandleNullPropagationOption handleNullPropagation = HandleNullPropagationOption.Default
        , bool handleReferenceNavigationPropertyExpandFilter = false)
        {
            var ret = new EnableQueryAttribute
            {
                AllowedArithmeticOperators = settings.AllowedArithmeticOperators,
                AllowedLogicalOperators = settings.AllowedLogicalOperators,
                AllowedFunctions = settings.AllowedFunctions,
                AllowedQueryOptions = settings.AllowedQueryOptions,
                AllowedOrderByProperties = string.Join(',', settings.AllowedOrderByProperties),
                MaxAnyAllExpressionDepth = settings.MaxAnyAllExpressionDepth,
                MaxExpansionDepth = settings.MaxExpansionDepth,
                MaxNodeCount = settings.MaxNodeCount,
                MaxOrderByNodeCount = settings.MaxOrderByNodeCount,
                MaxSkip = settings.MaxSkip ?? maxSkip,
                MaxTop = settings.MaxTop ?? maxTop,
                PageSize = pageSize,
                //Order = settings.ord,
                EnableConstantParameterization = true,
                EnableCorrelatedSubqueryBuffering = true,
                EnsureStableOrdering = true,
                HandleNullPropagation = handleNullPropagation,
                HandleReferenceNavigationPropertyExpandFilter = handleReferenceNavigationPropertyExpandFilter
            };
            return ret;
        }

        /// <summary> Type-specific Validation Settings </summary>
        public static readonly Dictionary<Type, ODataValidationSettings> TypeSettings = new();

        public static HandleNullPropagationOption HandleNullPropagation = HandleNullPropagationOption.Default;

        public static void ValidateTyped<T>(this ODataQueryOptions<T> options)
        {
            if (TypeSettings.TryGetValue(typeof(T), out var typeSettings))
            {
                options.Validate(typeSettings);
            }
            Validate(options);
        }

        public static void Validate(ODataQueryOptions options)
        {
            options.Validate(Settings);
        }
    }
}
