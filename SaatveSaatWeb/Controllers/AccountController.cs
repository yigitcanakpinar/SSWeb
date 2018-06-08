using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaatveSaatWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        private SaatvesaatAccountDBEntities db = new SaatvesaatAccountDBEntities();

        public ActionResult Index()
        {

            return View();

        }

        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            try
            {
            string username = form["username"];
            string password = form["password"];


            var query = db.Username.FirstOrDefault(x => x.Username1  == username && x.Password == password);

            if (query != null)
            {
                Session["Admin"] = "1";
                return Redirect("~/SaatveSaat");
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }


        }

    }
}