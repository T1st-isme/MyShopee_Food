using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin
{
    public class ShopVM
    {
        public int MaTK { get; set; }
        public int MaShop { get; set; }
        public string TenShop { get; set; }
        public string DiaChi { get; set; }
        public int? SDT { get; set; }
        public string TinhTrang { get; set; }
        public string HoTen { get; set; }
        public string AnhDaiDien { get; set; }
        public string AnhBia { get; set; }
        public string AnhThucTe { get; set; }
        public string Email { get; set; }
        public string MoTa { get; set; }
    }
}