using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Laboratories.Web.Controllers
{
    public class LanguageController : Controller
    {


        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        #region Actions

        public ActionResult Change(string languageAbbreviation)
        {
            if (languageAbbreviation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbreviation);

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbreviation);
            }

            HttpCookie cookie = new HttpCookie("Language");

            cookie.Value = languageAbbreviation;

            Response.Cookies.Add(cookie);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        /// Returns true if the language is a right-to-left language. Otherwise, false. /// </summary> 
        /// 
        [NonAction]
        public static bool IsRighToLeft()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;
        }

        #endregion


    }
}