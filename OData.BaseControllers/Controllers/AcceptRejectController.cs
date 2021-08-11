using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Logging;

namespace ODataGeneric.BaseControllers.Controllers
{
    /// <summary> Adds Accept and Reject Methods to the common Get, PUT and Post Methods</summary>
    /// <remarks>
    /// * derived Service w configurable AutoAccept 
    /// 
    /// * <see cref="AcceptAsync"/>/<see cref="RejectAsync"/> is idempotent, unless someone modifies the Data again,
    /// so we model them with HTTP PUT.
    /// </remarks>
    public abstract class AcceptRejectController<T> : AChangeDataController<T> 
        where T : class
    {
        protected AcceptRejectController(ILogger logger) : base(logger)
        {
        }

        [Route("Review.Accept")]
        [HttpPost("Review.Accept")]
        public virtual IActionResult AcceptChanges(ODataActionParameters parameters) 
            => ProcessRecordsWithKeys(parameters);

        [Route("Review.Reject")]
        [HttpPost("Review.Reject")]
        public virtual IActionResult RejectChanges(ODataActionParameters parameters)
            => ProcessRecordsWithKeys(parameters);

        protected IActionResult ProcessRecordsWithKeys(ODataActionParameters parameters)
        {
            if (!parameters.TryGetValue("keys", out object? keyValues))
            {
                return BadRequest();
            }

            var keys = (int[])keyValues;

            return Ok(keys.Length);
        }

    }
}
