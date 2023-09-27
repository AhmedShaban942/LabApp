using System.Web;
using System.Web.Mvc;

namespace Laboratories.Web.Models
{
    public class PermClass : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
         if (HttpContext.Current.Session["UserInfo"] == null )
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                        {"controller","Account" },
                        {"Action","Index" }
                });
            }
        }
    }
  
}