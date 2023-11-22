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
            return View("SuccessView");
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
                price = "1",
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
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "VNĐ",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
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
