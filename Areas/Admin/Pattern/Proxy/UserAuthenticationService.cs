using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Pattern.UserAuthenticationService
{
    public interface IUserAuthenticationService
    {
        User Authenticate(string username, string password);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private DBShopeeFoodEntities _db;

        public UserAuthenticationService(DBShopeeFoodEntities db)
        {
            _db = db;
        }

        public User Authenticate(string username, string password)
        {
            var mapTK = new Areas.Admin.map.mapTaiKhoan().ChiTiet(username, password);
            return mapTK;
        }
    }
}