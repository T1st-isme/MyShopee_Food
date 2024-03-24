using Shopee_Food.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserSubControllerGetList
    {
        private readonly DBShopeeFoodEntities _db;

        public UserSubControllerGetList(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public List<User> GetAllUser()
        {
            return _db.Users.ToList();
        }
    }
}
