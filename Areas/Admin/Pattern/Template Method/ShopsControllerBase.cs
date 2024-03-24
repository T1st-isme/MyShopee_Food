using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace Shopee_Food.Areas.Admin.Pattern.Template_Method
{
    public abstract class ShopControllerBase : Controller
    {
        private readonly DBShopeeFoodEntities _db = new DBShopeeFoodEntities();

        public ActionResult Index()
        {
            var shop = GetAllShop();
            return View(GetViewName(), shop);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaTK = new SelectList(_db.Users, "MaTK", "TaiKhoan");
            return View();
        }

        [HttpPost]
        [Route("Admin/Shops/Create")]
        public  ActionResult Create(Shop shop)
        {
            if (ModelState.IsValid)
            {
                AddShop(shop);
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(_db.Users, "MaTK", "TaiKhoan", shop.MaTK);
            return View(shop);
        }
        

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var Shop = GetShopById(id);
            return View(Shop);
        }

        [HttpPost]
        public ActionResult Edit(Shop shop)
        {
            UpdateShop(shop);
            return RedirectToAction("Index");

        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            DeleteShop(id);
            return RedirectToAction("Index");
        }

        protected abstract string GetViewName();
        protected abstract List<Shop> GetAllShop();
        protected abstract Shop GetShopById(string id);
        protected abstract ActionResult AddShop(Shop shop);
        protected abstract ActionResult UpdateShop(Shop shop);
        protected abstract ActionResult DeleteShop(int id);
    }
}
