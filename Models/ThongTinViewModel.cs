using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Models
{
    public class ThongTinViewModel
    {
        public Shop Shop { get; set; }
        public  User user { get; set; }
        public ThanhToan thanhToan { get; set; }

    }
}