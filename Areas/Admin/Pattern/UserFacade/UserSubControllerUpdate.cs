using Shopee_Food.Models;
using System;
using System.Linq;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserSubControllerUpdate
    {
        private readonly DBShopeeFoodEntities _db;

        public UserSubControllerUpdate(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _db.Users.Find(user.MaTK);
            if (existingUser == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            // Cập nhật thông tin người dùng
            existingUser.TaiKhoan = user.TaiKhoan;
            existingUser.MatKhau = user.MatKhau;
            existingUser.HoTen = user.HoTen;
            existingUser.DiaChi = user.DiaChi;
            existingUser.SDT = user.SDT;
            existingUser.Email = user.Email;
            existingUser.GioiTinh = user.GioiTinh;
            
            // Cập nhật các trường thông tin khác nếu có

            _db.SaveChanges();
        }

    }
}
