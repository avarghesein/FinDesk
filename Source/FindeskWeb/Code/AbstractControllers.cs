using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Findesk.Web.Code
{
    public class AbstractApiController : ApiController
    {
        private WebElement _webElement;

        protected WebElement WebElement
        {
            get
            {
                return _webElement ?? (_webElement = new WebElement());
            }
        }
    };

    public class AbstractController : Controller
    {
        private WebElement _webElement;

        protected WebElement WebElement
        {
            get
            {
                return _webElement ?? (_webElement = new WebElement());
            }
        }
    };
};