using Shopee_Food.App_Start;
using Shopee_Food.Areas.Admin.Pattern.ProductAD;
using Shopee_Food.Models;
using Shopee_Food.Pattern.UserAuthenticationService;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Shopee_Food.Areas.Admin.Controllers
{
    public class AdHomeController : Controller
    {
        public IUserAuthenticationService _userAuthService;
        public IProductADRepository _productADRepository;
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        public AdHomeController()
        {
            this._productADRepository = new ProductADRepository(new DBShopeeFoodEntities());
            this._userAuthService = new UserAuthenticationService(new DBShopeeFoodEntities());
        }

        [AdminAuth(MaCV = 1)]
        public ActionResult Index()
        {
            var pro = from s in _productADRepository.GetProduct()
                      select s;
            return View(pro);
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

            // Sử dụng proxy để kiểm tra đăng nhập
            var mapTK = new Areas.Admin.map.mapTaiKhoan().ChiTiet(username, password);
            if (authenticatedUser == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
                ViewBag.TaiKhoan = taiKhoan;
                return View();
            }

            Session["user"] = authenticatedUser;

            return RedirectToAction("Index", "AdHome");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "AdHome");
        }
    }
}