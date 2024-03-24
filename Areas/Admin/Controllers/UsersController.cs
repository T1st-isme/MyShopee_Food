using System.Net;
using System.Web.Mvc;
using Shopee_Food.Models;
using Shopee_Food.Areas.Admin.Pattern.UserFacade;

namespace Shopee_Food.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        private UserFacade _userFacade;

        public UsersController()
        {
            this._userFacade = new UserFacade(new DBShopeeFoodEntities());
        }

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = _userFacade.GetAllUser();
            return View(users);
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,TaiKhoan,MatKhau,HoTen,DiaChi,SDT,Email,GioiTinh,MaCV")] User user)
        {
            if (ModelState.IsValid)
            {
                _userFacade.AddUser(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", user.ChucVu);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", user.ChucVu);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTK,TaiKhoan,MatKhau,HoTen,DiaChi,SDT,Email,GioiTinh,MaCV")] User user)
        {
            if (ModelState.IsValid)
            {
                _userFacade.UpdateUser(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", user.ChucVu);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            _userFacade.DeleteUser(id);
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