using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Pattern.Product
{
    public interface IProductBuilder
    {
        //IProductBuilder ByCategory(string category);
        IProductBuilder ByPrice(decimal? minPrice, decimal? maxPrice);

        IEnumerable<SanPham> Build();
    }

    public class ProductBuilder : IProductBuilder
    {
        private IQueryable<SanPham> _query;

        public ProductBuilder(IQueryable<SanPham> initialQuery)
        {
            _query = initialQuery;
        }

        //public IProductBuilder ByCategory(string category)
        //{
        //    if (!string.IsNullOrEmpty(category))
        //    {
        //        _query = _query.Where(p => p.Loai == category);
        //    }
        //    return this;
        //}

        public IProductBuilder ByPrice(decimal? minPrice, decimal? maxPrice)
        {
            _query = _query.Where(p => p.DonGia >= minPrice && p.DonGia <= maxPrice);
            return this;
        }

        public IEnumerable<SanPham> Build()
        {
            return _query.ToList();
        }
    }
}