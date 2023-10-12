using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Controllers
{
    public class CartController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        // GET: Cart
        public ActionResult Index()
        {
            List<MatHangMua> gioHang = getCarts();
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["totalCart"] = 0;
                return View("CartNoProduct");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            //Session["totalCart"] = (Session["GioHang"] as List<MatHangMua>).Count;
            Session["totalCart"] = gioHang.Count;
            return View(gioHang);
        }

        public List<MatHangMua> getCarts()
        {
            //var getTempUser = db.Users.FirstOrDefault(x => x.MaTK == 6);
            //if (getTempUser != null)
            //    Session["user"] = getTempUser;

            var getUser = Session["user"] as User;

            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;

            if (getUser != null)
            {
                var getListCart = db.GioHangs.Where(x => x.id_user == getUser.MaTK).ToList();
                List<MatHangMua> listCart = new List<MatHangMua>();
                if (getListCart != null || getListCart.Count > 0)
                {
                    foreach (var i in getListCart)
                    {
                        MatHangMua sp = new MatHangMua(Convert.ToInt32(i.MaSP));
                        sp.Amount = (int)(i.quantity);
                        listCart.Add(sp);
                    }
                    Session["GioHang"] = listCart;
                    return listCart;
                }
            }

            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        public ActionResult AddProduct(int MaSP)
        {
            var getuser = Session["user"] as User;
            if (getuser != null)
            {
                var checkProduct = db.GioHangs.Where(x => x.MaSP == MaSP).FirstOrDefault();
                if (checkProduct == null)
                {
                    var sp = new GioHang()
                    {
                        id_user = getuser.MaTK,
                        MaSP = MaSP,
                        quantity = 1,
                    };
                    db.GioHangs.Add(sp);
                    db.SaveChanges();
                }
                else
                {
                    checkProduct.quantity++;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Cart");
            }

            List<MatHangMua> gioHang = getCarts();
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (sanPham == null)
            {
                sanPham = new MatHangMua(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.Amount++;
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult DeleteProduct(int MaSP)
        {
            var getuser = Session["user"] as User;
            if (getuser != null)
            {
                var checkProduct = db.GioHangs.Where(x => x.MaSP == MaSP).FirstOrDefault();
                if (checkProduct != null)
                {
                    var sp = new GioHang()
                    {
                        id_user = getuser.MaTK,
                        MaSP = MaSP,
                        quantity = 1,
                    };
                    db.GioHangs.Remove(checkProduct);
                    db.SaveChanges();
                }
            }
            else
            {
                List<MatHangMua> gioHang = getCarts();
                var sanpham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
                if (sanpham != null)
                {
                    gioHang.RemoveAll(s => s.MaSP == MaSP);
                    return RedirectToAction("Index");
                }
                if (gioHang.Count == 0)
                    return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdateCart(int MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = getCarts();
            var sanpham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (sanpham != null)
            {
                sanpham.Amount = SoLuong;
                //db.carts.FirstOrDefault(x => x.MaDT == MaDT).quantity = SoLuong;

                var getuser = Session["user"] as User;

                if (getuser != null)
                {
                    //get cart of user when after login
                    var getcart = db.GioHangs.Where(x => x.id_user == getuser.MaTK).ToList();
                    db.GioHangs.FirstOrDefault(x => x.id_user == getuser.MaTK && x.MaSP == MaSP).quantity = SoLuong;
                    getcart.FirstOrDefault(x => x.MaSP == MaSP).quantity = SoLuong;
                    db.SaveChanges();
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index"); ;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = getCarts();
            if (gioHang != null)
            {
                TongTien = gioHang.Sum(sp => sp.Total());
            }
            return TongTien;
        }

        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = getCarts();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.Amount);
            return tongSL;
        }
    }
}