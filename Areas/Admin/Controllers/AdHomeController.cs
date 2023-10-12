using Shopee_Food.App_Start;
using System.Web.Mvc;
using System.Web.Security;

namespace Shopee_Food.Areas.Admin.Controllers
{
    public class AdHomeController : Controller
    {
        [AdminAuth(MaCV = 3)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string taiKhoan, string passWord)
        {
            //Check tai khoan va mat khau isEmpty() => return LoginPage
            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(passWord))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var mapTK = new map.mapTaiKhoan().ChiTiet(taiKhoan, passWord);

            if (mapTK == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                ViewBag.TaiKhoan = taiKhoan;
                return View();
            }

            Session["user"] = mapTK;

            return Redirect("/Admin/AdHome");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "AdHome");
        }
    }
}