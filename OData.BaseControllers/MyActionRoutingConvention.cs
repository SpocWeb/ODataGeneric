using Microsoft.AspNetCore.OData.Routing.Conventions;

namespace ODataGeneric.BaseControllers
{
    public class MyActionRoutingConvention : ActionRoutingConvention 
    {
        public override bool AppliesToController(ODataControllerActionContext context)
        {
            return base.AppliesToController(context);
        }

        public override bool AppliesToAction(ODataControllerActionContext context)
        {
            return base.AppliesToAction(context);
        }
    }
}