using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Models
{
    public class MatHangMua
    {
        DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        public int MaSP { get; set; }
        public string TenSp { get; set; }
        public string img { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double Total()
        {
            return Price * Amount;
        }

        public MatHangMua(int MaSP)
        {
            var getSP = db.SanPhams.FirstOrDefault(x => x.MaSP == MaSP);
            this.MaSP = MaSP;
            this.TenSp = getSP.TenSP.ToString();
            //this.TenSp = "Com suon";
            //this.img = getSP.img
            this.Price = double.Parse(getSP.DonGia.ToString());
            this.Amount = 1;

        }
    }
}