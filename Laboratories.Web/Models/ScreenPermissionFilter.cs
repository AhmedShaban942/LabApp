using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratories.Web.Models
{
    public class ScreenPermissionFilter : ActionFilterAttribute
    {
        public int screenId { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var screenPremssion = (ScreenViewModel)HttpContext.Current.Session["screenPremssion"];
                if (!screenPremssion.ScreenesId.Contains(screenId))
                {
                    filterContext.Result = new RedirectToRouteResult(
                     new System.Web.Routing.RouteValueDictionary {
                        {"controller","Account" },
                        {"Action","Index" }
                 });
                }
            }
            catch (Exception ex)
            {

                string message = ex.Message;
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                        {"controller","Account" },
                        {"Action","Index" }
                });
            }

        }
    }
}