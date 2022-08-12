using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogWebApi.Base
{
    public class ResponseGuidAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("Response-Guid", Guid.NewGuid().ToString());
        }
    }
}
