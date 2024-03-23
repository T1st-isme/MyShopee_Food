using System.Linq;

namespace Shopee_Food.Models
{
    public class MatHangMua : ProductPrototype
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        public int MaSP { get; set; }
        public string TenSp { get; set; }
        public string img { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }

        public double Total()
        {
            return Price * Amount;
        }

        // Phương thức tạo bản sao sâu của đối tượng MatHangMua
        public override ProductPrototype Clone()
        {
            return new MatHangMua(MaSP)
            {
                MaSP = this.MaSP,
                TenSp = this.TenSp,
                img = this.img,
                Price = this.Price,
                Amount = this.Amount
            };
        }

        public MatHangMua(int MaSP)
        {
            var getSP = db.SanPhams.FirstOrDefault(x => x.MaSP == MaSP);
            this.MaSP = MaSP;
            this.TenSp = getSP.TenSP.ToString();
            //this.TenSp = "Com suon";
            //this.img = getSP.HinhSP.ToString();
            this.Price = double.Parse(getSP.DonGia.ToString());
            this.Amount = 1;
        }
    }
}