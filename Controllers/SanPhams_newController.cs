using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopee_Food.Models;

namespace Shopee_Food.Controllers
{
    public class SanPhams_newController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        public PartialViewResult CategoryPartial()
        {
            var cateList = db.DanhMucs.ToList();
            return PartialView(cateList);
        }

        // GET: SanPhams_new
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.DanhMuc).Include(s => s.Shop);
            return View(sanPhams.ToList());
        }

        //sản phẩm
        public ActionResult Index_user(int? category, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        {
            //Tạo Products và có tham chiếu đến category
            var products = db.SanPhams.Include(p => p.DanhMuc);
            //tìm kiếm chuỗi truy vẫn theo category
            if (category == null)
            {
                products = db.SanPhams.OrderByDescending(x => x.TenSP);
            }
            else
            {
                products = db.SanPhams.OrderByDescending(x => x.MaDM).Where(x => x.MaDM == category);
            }
            //tìm theo tên
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.TenSP.ToLower().Contains(SearchString));
            }
            return View(products.ToList());
        }

        // GET: SanPhams_new/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams_new/Create
        public ActionResult Create()
        {
            int id = int.Parse(Session["IDCus"].ToString());
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc");
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc");
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop");
            return RedirectToAction("GetOrdersByShop", "Shops_new");
        }

        // POST: SanPhams_new/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,MaShop,MaDM,TenSP,Loai,DonGia,Soluongdaban,Soluongton,MoTa")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return RedirectToAction("GetOrdersByShop", "Shops_new");
        }

        // GET: SanPhams_new/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return View(sanPham);
        }

        // POST: SanPhams_new/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,MaShop,MaDM,TenSP,Loai,DonGia,Soluongdaban,Soluongton")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return View(sanPham);
        }

        // GET: SanPhams_new/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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