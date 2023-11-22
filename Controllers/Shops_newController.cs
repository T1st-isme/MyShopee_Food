using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shopee_Food.Models;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Web.Configuration;
using System.Web.Razor.Tokenizer.Symbols;
using Shopee_Food.Areas.Admin;

namespace Shopee_Food.Controllers
{
    public class Shops_newController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        public ActionResult dangki()
        {
            return View();
        }

        //max min
        public decimal GetMinPrice(int shopId)
        {
            var minPrice = db.SanPhams
                .Where(sp => sp.MaShop == shopId)
                .Min(sp => sp.DonGia);
            return minPrice;
        }

        public decimal GetMaxPrice(int shopId)
        {
            var maxPrice = db.SanPhams
                .Where(sp => sp.MaShop == shopId)
                .Max(sp => sp.DonGia);
            return maxPrice;
        }

        // GET: Shops_new
        public ActionResult Index()
        {
            var shop = db.Shops.Include(s => s.User);
            return View(shop.ToList());
        }

        public ActionResult Index_user()
        {
            var shop = db.Shops.Include(s => s.User);
            return View(shop.ToList());
        }

        // GET: Shops_new/Details/5
        public ActionResult Details(int id, String keyword)
        {
            var shop = db.Shops.FirstOrDefault(s => s.MaShop == id);

            if (shop == null)
            {
                return HttpNotFound();
            }
            var products = db.SanPhams.Where(p => p.MaShop == id).ToList();
            // Kiểm tra và cung cấp giá trị mặc định cho keyword nếu nó là null
            keyword = keyword ?? string.Empty;

            // Nếu có từ khóa tìm kiếm, thực hiện tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.TenSP.ToLower().Contains(keyword)).ToList();
            }
            ViewBag.Shop = shop;
            ViewBag.Products = products;
            return View();
        }

        public ActionResult Details_Shop(int id, String keyword)
        {
            var shop = db.Shops.FirstOrDefault(s => s.MaShop == id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            var products = db.SanPhams.Where(p => p.MaShop == id).ToList();
            // Kiểm tra và cung cấp giá trị mặc định cho keyword nếu nó là null
            keyword = keyword ?? string.Empty;
            // Nếu có từ khóa tìm kiếm, thực hiện tìm kiếm sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.TenSP.ToLower().Contains(keyword)).ToList();
            }
            ViewBag.Shop = shop;
            ViewBag.Products = products;
            return View();
        }

        //tìm kiếm sản phẩm
        [HttpPost]
        public ActionResult Search(int? id, string keyword)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var shop = db.Shops.FirstOrDefault(s => s.MaShop == id);

            if (shop == null)
            {
                return HttpNotFound();
            }

            var products = db.SanPhams
                .Where(p => p.MaShop == id && p.TenSP.Contains(keyword))
                .ToList();

            ViewBag.Shop = shop;
            ViewBag.Products = products;

            return View("Details");
        }

        public ActionResult GetOrdersBySHOP(int IDCus)
        {
            var orders = db.Shops
              .Where(o => o.MaTK == IDCus)
              .ToList();
            if (orders.Count == 0)
            {
                ViewBag.NoShop = true;
            }
            else
            {
                ViewBag.NoShop = false;
                ViewBag.Shops = orders;
            }
            return View(orders);
        }

        //giới hạn
        public ActionResult Create()
        {
            int maUser = (int)Session["IDCus"];
            var user = db.Users.FirstOrDefault(u => u.MaTK == maUser);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra số lượng cửa hàng đã tạo
            int shopCount = db.Shops.Count(s => s.MaTK == maUser);
            int maxShopCount = 1; // Giới hạn số lượng cửa hàng cho mỗi tài khoản (có thể thay đổi theo ý muốn)
            if (shopCount >= maxShopCount)
            {
                // Nếu đã đạt đến giới hạn, chuyển hướng hoặc hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Bạn đã đạt đến giới hạn số lượng cửa hàng.";
                return View("Error_check_createShop");
            }

            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", maUser);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaShop,TenShop,DanhGia,TinhTrang,SoLuongSanPham,DoanhThu,AnhBia,AnhDaiDien,AnhThucTe,MaTK,MoTa,HinhMenu,DiaChiShop")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                int maUser = (int)Session["IDCus"];
                var user = db.Users.FirstOrDefault(u => u.MaTK == maUser);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra số lượng cửa hàng đã tạo
                int shopCount = db.Shops.Count(s => s.MaTK == maUser);
                int maxShopCount = 5; // Giới hạn số lượng cửa hàng cho mỗi tài khoản (có thể thay đổi theo ý muốn)
                if (shopCount >= maxShopCount)
                {
                    // Nếu đã đạt đến giới hạn, chuyển hướng hoặc hiển thị thông báo lỗi
                    // Ví dụ:
                    ViewBag.ErrorMessage = "Bạn đã đạt đến giới hạn số lượng cửa hàng.";
                    return View("Error_check_createShop");
                }

                shop.MaTK = maUser;

                db.Shops.Add(shop);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = shop.MaShop });
            }

            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", shop.MaTK);
            return View(shop);
        }

        //// GET: Shops_new/Create
        //public ActionResult Create()
        //{
        //    ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan");
        //    // Lưu danh sách quốc gia vào Session
        //    return View();
        //}

        //// POST: Shops_new/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaShop,TenShop,DanhGia,TinhTrang,SoLuongSanPham,DoanhThu,AnhBia,AnhDaiDien,AnhThucTe,MaTK,MoTa,HinhMenu,DiaChiShop")] Shop shop)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int maUser = (int)Session["IDCus"];
        //        shop.MaTK = maUser;

        //        db.Shops.Add(shop);
        //        db.SaveChanges();
        //        return RedirectToAction("edit", new { id = shop.MaShop });

        //    }
        //    // Lưu danh sách quốc gia vào Session
        //    ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", shop.MaTK);
        //    return View(shop);
        //}
        // GET: Shops_new/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", shop.MaTK);

            return View(shop);
        }

        // POST: Shops_new/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaShop,TenShop,DanhGia,TinhTrang,SoLuongSanPham,DoanhThu,AnhBia,AnhDaiDien,AnhThucTe,MaTK,MoTa,HinhMenu")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Users_new", new { id = shop.MaTK });
            }
            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", shop.MaTK);
            return View(shop);
        }

        // GET: Shops_new/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shops.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // POST: Shops_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shop shop = db.Shops.Find(id);
            db.Shops.Remove(shop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}