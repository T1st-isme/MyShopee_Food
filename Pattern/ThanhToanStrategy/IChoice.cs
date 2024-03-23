using Amazon.Runtime.Internal;
using PayPal.Api;
using Shopee_Food.Controllers;
using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Pattern.ThanhToanStrategy
{
    public interface IChoice
    {
        void Choice(HttpContextBase context, string DiaDiemGiaoHang, List<MatHangMua> cart);
    }

    internal class PayPalChoice : IChoice
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        private Func<double> _tinhTongTien;
        private Func<double> _tinhTongTienvnd;

        public PayPalChoice(Func<double> tinhTongTien, Func<double> tinhTongTienvnd)
        {
            _tinhTongTien = tinhTongTien;
            _tinhTongTienvnd = tinhTongTienvnd;
        }

        public void Choice(HttpContextBase context, string DiaDiemGiaoHang, List<MatHangMua> cart)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = context.Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + "/Cart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
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
                    context.Session.Add(guid, createdPayment.id);
                    context.Response.Redirect(paypalRedirectUrl, true);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = context.Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, context.Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return;
                    }
                    else
                    {
                        string user = (string)context.Session["UserName"];
                        User getUser = db.Users.FirstOrDefault(u => u.TaiKhoan == user);
                        //Help me save info order
                        cart = context.Session["GioHang"] as List<MatHangMua>;

                        decimal totalM = 0;

                        foreach (var i in cart)
                        {
                            totalM += i.Amount * (decimal)i.Price;
                        }
                        context.Session["total"] = totalM;

                        var DonHangTemp = new DonHang
                        {
                            MaTK = getUser.MaTK,
                            MaSP = 0,
                            DiaDiemGiaoHang = DiaDiemGiaoHang,
                            NgayDat = DateTime.Now,
                            NgayGiao = DateTime.Now.AddDays(3), // Assuming delivery after 3 days
                            TrangThai = "pending",
                            TongTien = context.Session["total"] as decimal?,
                        };

                        db.DonHangs.Add(DonHangTemp);
                        db.SaveChanges();

                        foreach (var i in cart)
                        {
                            decimal totalTemp = i.Amount * (decimal)i.Price;
                            var CTDH = new DonHangChiTiet
                            {
                                MaDH = DonHangTemp.MaDH,
                                MaSP = i.MaSP,
                                Soluong = i.Amount,
                                Tongtien = (int?)totalTemp,
                            };

                            db.DonHangChiTiets.Add(CTDH);
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        var thanhToan = new ThanhToan
                        {
                            MaTK = getUser.MaTK,
                            MaDH = DonHangTemp.MaDH,
                            PTTT = "PayPal",
                            NgayThanhToan = DateTime.Now,
                        };
                        db.ThanhToans.Add(thanhToan);
                        db.SaveChanges();

                        if (getUser != null)
                        {
                            var getcart = db.GioHangs.Where(x => x.id_user == getUser.MaTK).ToList();
                            db.GioHangs.RemoveRange(getcart);
                            db.SaveChanges();
                        }

                        cart.Clear();
                        context.Session["GioHang"] = cart;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("<h1>Error occured: </h1>" + ex.Message);
            }
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
            var totalAmount = _tinhTongTienvnd().ToString("F2", CultureInfo.InvariantCulture);
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
            return payment.Create(apiContext);
        }
    }

    internal class CODChoice : IChoice
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        private readonly Func<double> _tinhTongTien;

        public CODChoice(Func<double> tinhTongTien)
        {
            _tinhTongTien = tinhTongTien;
        }

        public void Choice(HttpContextBase context, string DiaDiemGiaoHang, List<MatHangMua> cart)
        {
            string getUser = (string)context.Session["UserName"];
            User user = db.Users.FirstOrDefault(u => u.TaiKhoan == getUser);

            // Handle the case where the user is not found in the session
            if (getUser == null)
            {
                // Redirect to the login page or handle appropriately
                return;
            }

            // Retrieve the cart from the session
            cart = context.Session["GioHang"] as List<MatHangMua>;

            // Handle the case where the cart is empty or not found in the session
            if (cart == null || !cart.Any())
            {
                // Redirect to the home page or handle appropriately
                return;
            }

            // Calculate the total amount of the order
            decimal totalM = cart.Sum(item => item.Amount * (decimal)item.Price);
            context.Session["total"] = totalM;

            // Create a new order
            var order = new DonHang
            {
                MaTK = user.MaTK,
                MaSP = 0,
                DiaDiemGiaoHang = DiaDiemGiaoHang,
                NgayDat = DateTime.Now,
                NgayGiao = DateTime.Now.AddDays(3), // Assuming delivery after 3 days
                TrangThai = "pending",
                TongTien = totalM
            };

            // Add the order to the database
            db.DonHangs.Add(order);
            db.SaveChanges();

            // Add order details for each item in the cart
            foreach (var item in cart)
            {
                decimal totalTemp = item.Amount * (decimal)item.Price;
                var orderDetail = new DonHangChiTiet
                {
                    MaSP = item.MaSP,
                    Soluong = item.Amount,
                    Tongtien = (int?)totalTemp,
                    MaDH = order.MaDH // Link the order detail to the order
                };

                // Add the order detail to the database
                db.DonHangChiTiets.Add(orderDetail);
            }

            // Save all changes to the database
            db.SaveChanges();

            // Clear the cart and update session variables
            cart.Clear();
            context.Session["GioHang"] = cart;
            context.Session["totalCart"] = 0;
        }
    }
}