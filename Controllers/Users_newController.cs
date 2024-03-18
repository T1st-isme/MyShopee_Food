using Shopee_Food.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace Shopee_Food.Controllers
{
    public class Users_newController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        //dang ki
        public ActionResult LogOutUserAD()
        {
            Session.Abandon();
            return RedirectToAction("Index", "LoginUser");
        }

        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User _user)
        {
            if (ModelState.IsValid)
            {
                var check_ID = db.Users.Where(s => s.MaTK == _user.MaTK).SingleOrDefault();
                if (check_ID == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    _user.HoTen = _user.TaiKhoan;
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("LoginCus", "Users_new");
                }
                else
                {
                    ViewBag.ErrorRegister = "Tài khoản đã tồn tại";
                    return View();
                }
            }

            return View();
        }

        //login
        [HttpGet]
        public ActionResult LoginCus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginCus(User _cus)
        {
            // check the user based on the username
            var check = db.Users.FirstOrDefault(s => s.TaiKhoan == _cus.TaiKhoan && s.MatKhau == _cus.MatKhau);
            if (check == null)  // no user found
            {
                ViewBag.ErrorInfo = "Tài khoản hoặc mật khẩu không đúng";
                return View("LoginCus");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["IDCus"] = check.MaTK;
                Session["Passwod"] = check.MatKhau;
                Session["NameCus"] = check.HoTen;
                Session["PhoneCus"] = check.SDT;
                Session["UserName"] = check.TaiKhoan;

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session["UserName"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginCus", "Users_new");
        }

        // GET: Users_new
        public ActionResult Index()
        {
            //var users = db.Users.Include(u => u.ChucVu);
            return View(db.Users.ToList());
        }

        // GET: Users_new/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        public ActionResult ShoppingInfo()
        {
            return View();
        }

        // GET: Users_new/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users_new/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TaiKhoan,MatKhau,HoTen,DiaChi,SDT,Email,GioiTinh,ChucVu,CCCD")] User users)
        {
            if (ModelState.IsValid)
            {
                var check_ID = db.Users.Where(s => s.MaTK == users.MaTK).FirstOrDefault();
                if (check_ID == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("LoginCus");
                }
                else
                {
                    ViewBag.ErrorRegister = "This ID is exist";
                    return View();
                }
            }
            //ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", users.MaCV);
            return View(users);
        }

        // GET: Users_new/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", users.MaCV);
            return View(users);
        }

        // POST: Users_new/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TaiKhoan,MatKhau,HoTen,DiaChi,SDT,Email,GioiTinh,MaCV,CCCD")] User users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "ThanhToans_new", new { id = users.MaTK });
            }
            //ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", users.MaCV);
            return View(users);
        }

        public ActionResult Edit_U(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", users.MaCV);
            return View(users);
        }

        // POST: Users_new/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_u([Bind(Include = "MaTK,TaiKhoan,MatKhau,HoTen,DiaChi,SDT,Email,GioiTinh,MaCV,CCCD")] User users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "ThanhToans_new", new { id = users.MaTK });
            }
            //ViewBag.MaCV = new SelectList(db.ChucVu, "MaCV", "TenCV", users.MaCV);
            return View(users);
        }

        // GET: Users_new/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User users = db.Users.Find(id);
            db.Users.Remove(users);
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