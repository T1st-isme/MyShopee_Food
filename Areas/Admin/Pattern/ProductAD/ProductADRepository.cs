using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.Pattern.ProductAD
{
    public class ProductADRepository : IProductADRepository, IDisposable
    {
        private DBShopeeFoodEntities context;

        public ProductADRepository(DBShopeeFoodEntities context)
        {
            this.context = context;
        }

        public IEnumerable<SanPham> GetProduct()
        {
            return context.SanPhams.ToList();
        }

        public SanPham GetProductByID(int? productId)
        {
            var product = context.SanPhams.Find(productId);
            if (product == null)
            {
                return null;
            }
            return context.SanPhams.Find(productId);
        }

        public void InsertProduct(SanPham product)
        {
            context.SanPhams.Add(product);
        }

        public void DeleteProduct(int productID)
        {
            SanPham product = context.SanPhams.Find(productID);
            context.SanPhams.Remove(product);
        }

        public void UpdateProduct(SanPham product)
        {
            context.Entry(product).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}