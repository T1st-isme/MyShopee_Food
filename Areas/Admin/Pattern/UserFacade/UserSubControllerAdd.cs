using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserSubControllerAdd
    {
        private readonly DBShopeeFoodEntities _db;

        public UserSubControllerAdd(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
            var existingUser = _db.Users.FirstOrDefault(u => u.TaiKhoan == user.TaiKhoan);
            if (existingUser != null)
            {
                throw new Exception("Người dùng đã tồn tại.");
            }

            // Thêm người dùng mới vào cơ sở dữ liệu
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}