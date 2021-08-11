using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ODataGeneric.BaseControllers.Controllers
{
    public abstract class AChangeDataController<T> : ADataController<T> where T : class
    {

        public IActionResult Put(string action, [FromBody] T entity)
        {
            return Ok();
        }

        public override IActionResult Put(int key, [FromBody] T entity)
        {
            return base.Put(key, entity);
        }

        protected AChangeDataController(ILogger logger)
            : base(logger)
        {
        }
    }
}