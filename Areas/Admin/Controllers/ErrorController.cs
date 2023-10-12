using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Admin/Error
        public ActionResult KoCoQuyen()
        {
            return View();
        }

        public ActionResult Index()
        {
            return Redirect("~/Admin/Error/KoCoQuyen");
        }
    }
}