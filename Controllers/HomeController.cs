using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Controllers
{
    public class HomeController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        public ActionResult Index()
        {
            var getShop = db.Shops.ToList();
            ViewBag.ShopSize = getShop.Count();

            Console.WriteLine(getShop.Count());
            return View(getShop);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}