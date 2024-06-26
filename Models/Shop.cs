//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopee_Food.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shop()
        {
            this.SanPhams = new HashSet<SanPham>();
        }
    
        public int MaShop { get; set; }
        public string TenShop { get; set; }
        public string DanhGia { get; set; }
        public string TinhTrang { get; set; }
        public Nullable<int> SoLuongSanPham { get; set; }
        public Nullable<decimal> DoanhThu { get; set; }
        public string AnhBia { get; set; }
        public string AnhDaiDien { get; set; }
        public string AnhThucTe { get; set; }
        public int MaTK { get; set; }
        public string HinhMenu { get; set; }
        public string DiaChiShop { get; set; }
        public string MoTa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
        public virtual User User { get; set; }
    }
}
