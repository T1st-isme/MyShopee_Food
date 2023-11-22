using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using Shopee_Food.Models;

namespace Shopee_Food.Controllers
{
    public class ThanhToans_newController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        // GET: ThanhToans_new

        public ActionResult UserDetails()
        {
            try
            {
                // Lấy thông tin user từ bảng Users
                int maTK = (int)Session["IDCus"];


                User user = GetUserDetails(maTK);

                // Lấy thông tin thanh toán của user từ bảng ThanhToan (nếu có)
                ThanhToan thanhToan = GetThanhToanDetails(maTK);

                // Lấy thông tin shop của user từ bảng Shop (nếu có)
                Shop shop = GetShopDetails(maTK);

                // Gửi thông tin user, thanh toán và shop tới View để hiển thị
                ViewBag.User = user;
                ViewBag.ThanhToan = thanhToan;
                ViewBag.Shop = shop;

                return View();
            }
            catch (Exception ex)
            {
       
            }

            return RedirectToAction("Index", "ThanhToans_new"); // Hoặc chuyển hướng tới một trang thông báo lỗi khác
        }

        // Hàm để lấy thông tin user từ bảng Users (hoặc sử dụng Entity Framework)
        private User GetUserDetails(int maTK)
        {
            // Sử dụng Entity Framework hoặc phương pháp truy vấn cơ sở dữ liệu khác để lấy thông tin user
            // Ví dụ sử dụng Entity Framework:
            using (DBShopeeFoodEntities context = new DBShopeeFoodEntities()) // Thay đổi YourDbContext thành DbContext của bạn
            {

                return context.Users.FirstOrDefault(u => u.MaTK == maTK);
            }
        }

        // Hàm để lấy thông tin thanh toán của user từ bảng ThanhToan (hoặc sử dụng Entity Framework)
        private ThanhToan GetThanhToanDetails(int maTK)
        {
            // Tương tự như GetUserDetails, lấy thông tin thanh toán từ cơ sở dữ liệu
            using (DBShopeeFoodEntities context = new DBShopeeFoodEntities()) // Thay đổi YourDbContext thành DbContext của bạn
            {
                return context.ThanhToans.FirstOrDefault(u => u.MaTK == maTK);
            }
        }

        // Hàm để lấy thông tin shop của user từ bảng Shop (hoặc sử dụng Entity Framework)
        private Shop GetShopDetails(int maTK)
        {
            // Tương tự như GetUserDetails, lấy thông tin shop từ cơ sở dữ liệu
            using (DBShopeeFoodEntities context = new DBShopeeFoodEntities()) // Thay đổi YourDbContext thành DbContext của bạn
            {
                return context.Shops.FirstOrDefault(u => u.MaTK == maTK);
            }
        }



        public ActionResult Index()
        {
            var thanhToan = db.ThanhToans.Include(t => t.User);
            return View(thanhToan.ToList());
        }

        // GET: ThanhToans_new/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // GET: ThanhToans_new/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ThanhToans_new/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTT,STK,PTTT,MaTK")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                int maUser = (int)Session["IDCus"];
                thanhToan.MaTK = maUser;
                db.ThanhToans.Add(thanhToan);
                db.SaveChanges();        
                return RedirectToAction("UserDetails", "ThanhToans_new", new { id = thanhToan.MaTK });
            }
            return View(thanhToan);
        }

        // GET: ThanhToans_new/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", thanhToan.MaTK);
            return View(thanhToan);
        }

        // POST: ThanhToans_new/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTT,STK,PTTT,MaTK")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thanhToan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaTK = new SelectList(db.Users, "MaTK", "TaiKhoan", thanhToan.MaTK);
            return View(thanhToan);
        }

        // GET: ThanhToans_new/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            if (thanhToan == null)
            {
                return HttpNotFound();
            }
            return View(thanhToan);
        }

        // POST: ThanhToans_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThanhToan thanhToan = db.ThanhToans.Find(id);
            db.ThanhToans.Remove(thanhToan);
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
