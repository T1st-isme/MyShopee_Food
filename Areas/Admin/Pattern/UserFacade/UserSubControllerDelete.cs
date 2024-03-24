using Shopee_Food.Models;
using System;
using System.Linq;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserSubControllerDelete
    {
        private readonly DBShopeeFoodEntities _db;

        public UserSubControllerDelete(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public void DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.MaTK == id);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
        }
    }
}
