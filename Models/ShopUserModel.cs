using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Models
{
    public class ShopUserModel
    {
        public List<User> Users { get; set; } // Danh sách User
        public List<Shop> Shops { get; set; } // Danh sách Shop

        //Shop
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
        public string MoTa { get; set; }
        //USER

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public Nullable<int> SDT { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public int MaCV { get; set; }
        public Nullable<int> CCCD { get; set; }
        public string HinhMenu { get; internal set; }

        public int MaTT { get; set; }
        public int MaKH { get; set; }
        public string STK { get; set; }
        public string PTTT { get; set; }
        public List<Shop> TaiKhoanList { get; set; } // Danh sách MaTK từ bảng Users
        public User UserModel { get; set; } // Thay ShopUserModel bằng tên lớp User của bạn
        public Shop ShopModel { get; set; } // Thay ShopModel bằng tên lớp Shop của bạn
        public User User { get; internal set; }
        public Shop Shop { get; internal set; }
    }
}