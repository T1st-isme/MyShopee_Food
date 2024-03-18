using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Pattern.Cate
{
    public interface IDanhMucRepository : IDisposable
    {
        IEnumerable<DanhMuc> GetDanhMuc();

        DanhMuc GetDanhMucByID(int? danhMucId);

        void InsertDanhMuc(DanhMuc danhMuc);

        void DeleteDanhMuc(int danhMucID);

        void UpdateDanhMuc(DanhMuc danhMuc);

        void Save();
    }
}