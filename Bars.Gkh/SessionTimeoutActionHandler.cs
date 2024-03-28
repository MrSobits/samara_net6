namespace Bars.Gkh
{
    using Bars.B4;
    using Bars.B4.Utils.Web;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class SessionTimeoutActionHandler : IActionExecuteHandler
    {
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var claimsPrincipal = filterContext.HttpContext.User;

            if (claimsPrincipal.Identity is not { IsAuthenticated: true })
            {
                // TODO: проверить
                if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.GetApplicationPath() != "/action/setup/login")
                {
                    throw new AuthorizationFailureException("AjaxAuthError");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext resultExecutingContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext resultExecutedContext)
        {
        }
    }
}