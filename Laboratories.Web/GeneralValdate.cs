using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratories.Web
{
    public static class GeneralValdate
    {
        public static bool  ValidateImage(HttpPostedFileBase image)
        {
            int MaxContentLength = 1024 * 1024 * 2;
            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

            var file = image as HttpPostedFileBase;

            if (file == null)
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                return false;

            }
            else if (file.ContentLength > MaxContentLength)
            {

                return false;
            }
            else
                return true;
        }
    }
}