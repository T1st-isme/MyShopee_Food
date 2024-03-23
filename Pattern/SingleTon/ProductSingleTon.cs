using Shopee_Food.Controllers;
using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Pattern.Product
{
    public class ProductSingleton
    {
        private static readonly Lazy<ProductSingleton> instance = new Lazy<ProductSingleton>(() => new ProductSingleton());

        public static ProductSingleton Instance => instance.Value;

        private List<SanPham> products;

        private ProductSingleton()
        {
            // Khởi tạo và load danh sách sản phẩm từ database hoặc nguồn dữ liệu khác ở đây
            products = new List<SanPham>();
        }

        public List<SanPham> GetProducts()
        {
            return products;
        }
    }
}