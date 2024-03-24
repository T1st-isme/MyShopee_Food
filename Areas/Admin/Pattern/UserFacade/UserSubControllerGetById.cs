using Shopee_Food.Models;
using System;
using System.Linq;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserSubControllerGetById
    {
        private readonly DBShopeeFoodEntities _db;

        public UserSubControllerGetById(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public User GetUserById(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.MaTK == id);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            return user;
        }
    }
}
