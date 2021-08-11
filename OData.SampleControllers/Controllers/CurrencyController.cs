using System;
using System.Threading.Tasks;
using ODataGeneric.BaseControllers.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Logging;
using ODataGeneric.SampleControllers.Entities;

namespace ODataGeneric.SampleControllers.Controllers
{
    public class CurrencyController : AcceptRejectController<Currency>
    {

        public CurrencyController(ILogger<CurrencyController> logger) : base(logger) { }

        //[HttpPost("/api/Currency/Review.Accept")]
        //public override Task<IActionResult> AcceptChanges(ODataActionParameters parameters) => base.AcceptChanges(parameters);

        [Route("api/Currency/Review.Reject")]
        public override IActionResult RejectChanges(ODataActionParameters parameters) => base.RejectChanges(parameters);
    }
}
