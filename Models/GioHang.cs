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
    
    public partial class GioHang
    {
        public int id_GH { get; set; }
        public Nullable<int> id_user { get; set; }
        public Nullable<int> MaSP { get; set; }
        public Nullable<int> quantity { get; set; }
    
        public virtual User User { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
