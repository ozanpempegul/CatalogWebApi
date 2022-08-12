using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogWebApi.Base
{
    public class ConsoledResourceFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("ConsoledResourceFilter - OnResourceExecuted");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("ConsoledResourceFilter - OnResourceExecuting");
        }
    }

    public class ConsoledActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("ConsoledActionFilter - OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            Console.WriteLine("ConsoledActionFilter - OnActionExecuting");
        }
    }

    public class ConsoledAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("ConsoledAuthorizationFilter - OnAuthorization");
        }
    }

    public class ConsoledResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {           
            Console.WriteLine("ConsoledResultFilter - OnResultExecuted");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Remove("Server");
            context.HttpContext.Response.Headers.Remove("X-AspNet-Version");
            context.HttpContext.Response.Headers.Remove("X-AspNetMvc-Version");
            context.HttpContext.Response.Headers.Remove("X-Powered-By");
            Console.WriteLine("ConsoledResultFilter - OnResultExecuting");
        }
    }

    // Global ActionFilter
    // Registered within the Startup Class
    // Executes for all Actions within the MVC
    public class ConsoledGlobalActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("ConsoledGlobalActionFilter - OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("ConsoledGlobalActionFilter - OnActionExecuting");
        }
    }
}
