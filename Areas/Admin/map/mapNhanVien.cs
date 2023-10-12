using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.map
{
    public class mapNhanVien
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();

        public List<NhanVien> DanhSach()
        {
            var dataNV = db.NhanViens.ToList();
            return dataNV;
        }

        public List<NhanVien> TimKiem(string diachi, string danhgia)
        {
            if (danhgia == "Tất cả") { danhgia = null; }
            //var dataNV = db.NhanViens.Where(n => n.User.HoTen.Contains(diachi.ToLower()) == true || string.IsNullOrEmpty(diachi)).ToList();
            var dataNV = (from item in db.NhanViens
                          where (item.User.HoTen.Contains(diachi.ToLower()) == true || string.IsNullOrEmpty(diachi))
                          && (danhgia == null || item.DanhGia == danhgia)
                          select item).ToList();
            return dataNV.OrderBy(n => n.User.HoTen).ToList();
        }
    }
}