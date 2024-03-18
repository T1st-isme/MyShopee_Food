using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.Pattern.ProductAD
{
    public interface IProductADRepository : IDisposable
    {
        IEnumerable<SanPham> GetProduct();

        SanPham GetProductByID(int? productID);

        void InsertProduct(SanPham product);

        void DeleteProduct(int productID);

        void UpdateProduct(SanPham product);

        void Save();
    }
}