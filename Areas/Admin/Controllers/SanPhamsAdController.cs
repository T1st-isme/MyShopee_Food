using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopee_Food.Areas.Admin.Pattern.ProductAD;
using Shopee_Food.Models;
using Shopee_Food.Pattern.FlyWeight;

namespace Shopee_Food.Areas.Admin.Controllers
{
    public class SanPhamsAdController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        private IProductADRepository _productADRepository;

        public SanPhamsAdController()
        {
            //Repository Pattern
            this._productADRepository = new ProductADRepository(new DBShopeeFoodEntities());
        }

        // GET: Admin/SanPhamsAd
        public ActionResult Index()
        {
            var pro = from s in _productADRepository.GetProduct()
                      select s;
            return View(pro.ToList());
        }

        // GET: Admin/SanPhamsAd/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _productADRepository.GetProductByID(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/SanPhamsAd/Create

        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc");
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc");
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop");
            return View();
        }

        // POST: Admin/SanPhamsAd/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,MaShop,MaDM,TenSP,Loai,DonGia,Soluongdaban,Soluongton")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _productADRepository.InsertProduct(sanPham);
                _productADRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return View(sanPham);
        }

        // GET: Admin/SanPhamsAd/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _productADRepository.GetProductByID(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return View(sanPham);
        }

        // POST: Admin/SanPhamsAd/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,MaShop,MaDM,TenSP,Loai,DonGia,Soluongdaban,Soluongton")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _productADRepository.UpdateProduct(sanPham);
                _productADRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ViewBag.MaShop = new SelectList(db.Shops, "MaShop", "TenShop", sanPham.MaShop);
            return View(sanPham);
        }

        // GET: Admin/SanPhamsAd/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = _productADRepository.GetProductByID(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhamsAd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SanPham pro = _productADRepository.GetProductByID(id);
                _productADRepository.DeleteProduct(id);
                _productADRepository.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            _productADRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}