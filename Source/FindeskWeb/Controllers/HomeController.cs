using Findesk.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Findesk.Web.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Policies()
        {
            return View();
        }

        [HttpGet]
        public FileContentResult GetThumbnail(string id)
        {
            try
            {
                byte[] cont = null;

                var doc = WebElement.Document.Get(id);

                if (doc != null)
                {
                    cont = WebElement.Document.Get(id).Content;
                }

                return new FileContentResult(cont, "image/jpg");
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }


        [HttpGet]
        public ActionResult DownloadDocument(string id)
        {
            try
            {
                byte[] cont = null;

                var doc = WebElement.Document.Get(id);

                if (doc != null)
                {
                    cont = WebElement.Document.Get(id).Content;

                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        // for example foo.bak
                        FileName = doc.Name,

                        // always prompt the user for downloading, set to true if you want 
                        // the browser to try to show the file inline
                        Inline = false,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());
                    return File(cont, doc.ContentType);
                }

                return null;
            }
            catch (Exception eX)
            {
                throw WebElement.HttpException(eX);
            }
        }
    }
}
