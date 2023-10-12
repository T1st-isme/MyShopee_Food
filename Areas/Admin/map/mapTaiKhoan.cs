using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.map
{
    public class mapTaiKhoan
    {
        public User ChiTiet(string taiKhoan, string passWord)
        {
            try
            {
                DBShopeeFoodEntities db = new DBShopeeFoodEntities();
                var data = db.Users.SingleOrDefault(m => m.TaiKhoan == taiKhoan && m.MatKhau == passWord);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}