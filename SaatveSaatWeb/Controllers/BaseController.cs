using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaatveSaatWeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() != "1")
            {
                filterContext.Result = new RedirectResult("~/Account/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}