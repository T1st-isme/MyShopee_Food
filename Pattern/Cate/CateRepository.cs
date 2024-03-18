using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Pattern.Cate
{
    public class CateRepository : IDanhMucRepository, IDisposable
    {
        private DBShopeeFoodEntities context;

        public CateRepository(DBShopeeFoodEntities context)
        {
            this.context = context;
        }

        public IEnumerable<DanhMuc> GetDanhMuc()
        {
            return context.DanhMucs.ToList();
        }

        public DanhMuc GetDanhMucByID(int? danhMucId)
        {
            var danhMuc = context.DanhMucs.Find(danhMucId);
            if (danhMuc == null)
            {
                return null;
            }
            return context.DanhMucs.Find(danhMucId);
        }

        public void InsertDanhMuc(DanhMuc danhMuc)
        {
            context.DanhMucs.Add(danhMuc);
        }

        public void DeleteDanhMuc(int danhMucID)
        {
            DanhMuc danhMuc = context.DanhMucs.Find(danhMucID);
            context.DanhMucs.Remove(danhMuc);
        }

        public void UpdateDanhMuc(DanhMuc danhMuc)
        {
            context.Entry(danhMuc).State = System.Data.Entity.EntityState.Modified;
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