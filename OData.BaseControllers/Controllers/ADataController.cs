using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace ODataGeneric.BaseControllers.Controllers
{
    /// <summary> Base-URL: ./odata/People(1)?$include=VinylRecords </summary>
    /// <remarks>
    /// Mostly OData Convention-based Routing.
    ///
    /// Basisklasse
    /// TODO: CP mit Sub-Entities 
    /// OData mit .Net Standard 
    /// * Authentication (OpenId Connect)
    /// * Authorization (MultiTenancy )
    /// * Logging
    /// * AutoAccept / 4 Eyes Principle 
    /// * Base-Service w/o Accept 
    /// * derived Service w configurable AutoAccept 
    /// I
    /// 
    /// </remarks>
    public abstract partial class ADataController<T> : ODataController where T : class
    {
        protected readonly ILogger _Logger;

        protected ADataController(ILogger logger)
        {
            _Logger = logger;
        }

        /// <summary> Query all People with $Page $Select $Include </summary>
        /// <remarks>
        /// Instead of using the <see cref="EnableQueryAttribute"/> 
        /// you could use <see cref="Get(ODataQueryOptions{T})"/>,
        /// but that has no equivalent to <see cref="EnableQueryAttribute.PageSize"/>. 
        /// </remarks>
        [EnableQuery(PageSize = ODataValidation.MaxPageSize)]
        public IActionResult Get() => Ok();

        /// <summary> Returns a single Result </summary>
        /// <remarks>
        /// Actually this is only a special PK-Syntax Query.
        /// <code> ./Counterparty(999)?... </code>
        /// 
        /// Similarly the UuId can be queried using a Filter Expression (without Quotes):
        /// <code> ./Counterparty?$filter=Uuid eq AB45E348-E647-477D-93EE-5F8298F76888  </code>
        /// Alternatively you can specify the Field Type explicitly:
        /// <code> ./Counterparty?$filter=Uuid eq guid'07a2c616-968b-4a73-97bc-031c401e0b07'</code>
        /// </remarks>
        public IActionResult Get(int key)
        {
            return Ok();
        }

    }
}
