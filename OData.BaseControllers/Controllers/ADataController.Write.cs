using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Deltas;

namespace ODataGeneric.BaseControllers.Controllers
{
    public abstract partial class ADataController<T>
    {
        /// <summary> Updates individual Properties on a single Record with <paramref name="key"/> </summary>
        /// <remarks>
        /// The Request Body is a partial Record with JSON-Syntax like this:
        /// ```{"FieldX":"valueX", ... "FieldZ":"valueZ", }```
        /// </remarks>
        /// <returns><see cref="NotFoundResult"/> when the Key does not exist</returns>
        //[HttpPatch(RouteFullKey)]
        public IActionResult Patch(int key, [FromBody] Delta<T> patch)
        {
            return NoContent();
        }

        /// <summary> Logical Deletion instead of physical </summary>
        //[HttpDelete(RouteFullKey)]
        public IActionResult Delete(int key)
        {
            return NoContent();
        }

        /// <summary> POST should create new Instances. But if an ID is passed, this updates an existing Entity. </summary>
        //[HttpPost(RouteFull)]
        //public Task<IActionResult> Post([FromBody] T entity) => Put(0, entity);
        public IActionResult Post([FromQuery] string action) => Ok();

        /// <summary> PUT should update existing Instances. But if no ID or its Default is passed, this creates a new Entity. </summary>
        public virtual IActionResult Put(int key, [FromBody] T entity)
        {
            if (!ModelState.IsValid) 
            { // ...but that is not compatible with the OData Controller 
                var sb = new StringWriter();
                foreach (var modelState in ModelState.Values) {
                    foreach (ModelError error in modelState.Errors) {
                        sb.WriteLine(error.ErrorMessage + " " + error.Exception);
                    }
                }

                return BadRequest(sb.ToString()); // ModelState);
            }

            CancellationToken cancelToken = CancellationToken.None;

            return Created(entity);
        }

    }
}
