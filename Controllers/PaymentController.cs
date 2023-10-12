using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Controllers
{
    public class PaymentController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PayCOD()
        {
            var getUser = Session["user"] as User;
            if (getUser == null)
            {
                return View("Error");
            }
            else
            {
                List<MatHangMua> cart = Session["GioHang"] as List<MatHangMua>;

                decimal totalM = 0;

                foreach (var i in cart)
                {
                    totalM += i.Amount * (decimal)i.Price;
                }
                Session["total"] = totalM;

                var DonHangTemp = new DonHang
                {
                    MaTK = getUser.MaTK,
                    DiaDiemGiaoHang = getUser.DiaChi,
                    NgayDat = DateTime.Now,
                    NgayGiao = DateTime.Now,
                    TrangThai = "pending",
                    //PTTT = false,
                    TongTien = Session["total"] as decimal?,
                };

                db.DonHangs.Add(DonHangTemp);
                db.SaveChanges();

                ViewBag.User = getUser;
                ViewBag.Order = DonHangTemp;
                ViewBag.ListOrderItem = cart;
                ViewBag.paymentType = "Thanh Toán khi Nhận Hàng";

                foreach (var i in cart)
                {
                    decimal totalTemp = i.Amount * (decimal)i.Price;
                    var CTDH = new DonHangChiTiet
                    {
                        //MaHD = DonHangTemp.MaDH,
                        MaSP = i.MaSP,
                        Soluong = i.Amount,
                        Tongtien = (int?)totalTemp,
                    };

                    db.DonHangChiTiets.Add(CTDH);
                    var getCart = db.GioHangs.FirstOrDefault(x => x.MaSP == i.MaSP);
                    db.GioHangs.Remove(getCart);
                    db.SaveChanges();
                }
                db.SaveChanges();
            }
            return View();
        }
    }
}