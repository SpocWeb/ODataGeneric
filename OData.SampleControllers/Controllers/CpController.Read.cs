using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.Extensions.Logging;
using ODataGeneric.BaseControllers.Controllers;
using ODataGeneric.SampleControllers.Entities;

namespace ODataGeneric.SampleControllers.Controllers
{

    public class CounterpartyController : AcceptRejectController<Counterparty>
    {

        public CounterpartyController(ILogger<CounterpartyController> logger) : base(logger) { }

        [HttpPost("/api/Counterparty/Review.Accept")]
        public override IActionResult AcceptChanges(ODataActionParameters parameters) => base.AcceptChanges(parameters);

        //[Route("api/Counterparty/Review.Reject")]
        //public override Task<IActionResult> RejectChanges(ODataActionParameters parameters) => base.RejectChanges(parameters);

    }
}