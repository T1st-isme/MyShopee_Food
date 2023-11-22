using MongoDB.Bson;
using MongoDB.Driver;
using PayPal.Api;
using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Controllers
{
	public class CartController : Controller
	{
		private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

		// GET: Cart
		public ActionResult ShowCart()
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
				return RedirectToAction("ShowCart", "Cart");
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
			return RedirectToAction("ShowCart", "Cart");
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
					return RedirectToAction("ShowCart");
				}
				if (gioHang.Count == 0)
					return RedirectToAction("ShowCart", "Cart");
			}

			return RedirectToAction("ShowCart");
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
			return RedirectToAction("ShowCart"); ;
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

		public ActionResult ApplyDiscountCode(FormCollection form)
		{
			List<MatHangMua> gioHang = getCarts();
			string discountCode = (string)form["discountCode"];

			// Kết nối đến MongoDB server
			var client = new MongoClient("mongodb+srv://admin:admin@cluster0.56wxy95.mongodb.net/"); // Thay đổi URL kết nối nếu cần thiết

			// Chọn cơ sở dữ liệu và collection
			var database = client.GetDatabase("ShopeeFood"); // Thay "ten_cua_database" bằng tên thực của cơ sở dữ liệu
			var collection = database.GetCollection<BsonDocument>("Voucher"); // Thay "ten_cua_collection" bằng tên thực của collection

			// Truy vấn để tìm voucher với MaVoucher là discountCode
			var filter = Builders<BsonDocument>.Filter.Eq("NameVoucher", discountCode);
			var check = collection.Find(filter).FirstOrDefault();

			if (check != null)
			{
				// Nếu tìm thấy voucher, lấy giá trị PhanTramDis
				int perCentDis = check.GetValue("Discount").AsInt32;

				// Lưu giá trị vào Session
				Session["perCentDis"] = perCentDis;

				// Gọi phương thức Total_price_after_dis() từ đối tượng cart

				decimal price_after_dis = (decimal)Total_price_after_dis(perCentDis);
				Session["PriceAfterDis"] = price_after_dis;
			}
			else
			{
				ViewBag.Message = "Mã voucher không tồn tại.";
			}

			return RedirectToAction("ShowCart", "Cart");
		}

		public double Total_price_after_dis(double perCentDis)
		{
			double totalPrice = TinhTongTien();

			double totalAfterDiscount = totalPrice - (totalPrice * perCentDis/100);

			return (double)totalAfterDiscount;

		}

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ThanhToans_new/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("CheckOut_Success");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var totalAmount = TinhTongTien().ToString("F2", CultureInfo.InvariantCulture);
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = totalAmount,
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0.00",
                shipping = "0.00",
                subtotal = totalAmount
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = totalAmount, // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}