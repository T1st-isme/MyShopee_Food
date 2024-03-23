using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopee_Food.Models;

namespace Shopee_Food.Controllers
{
    public interface Iterator
    {
        GioHang First();

        GioHang Next();

        bool IsDone { get; }
        GioHang CurrentItem { get; }
    }

    public class GetCartIterator : Iterator
    {
        private List<GioHang> _listGioHang;
        private int current = 0;
        private int step = 1;

        // constructor
        public GetCartIterator(List<GioHang> listGioHang)
        {
            _listGioHang = listGioHang;
        }

        public bool IsDone
        {
            get { return current >= _listGioHang.Count(); }
        }

        public GioHang CurrentItem => _listGioHang[current];

        public GioHang First()
        {
            current = 0;
            return _listGioHang[current];
        }

        public GioHang Next()
        {
            current += step;
            if (!IsDone) // chưa xong
                return _listGioHang[current]; // thì thằng kế tiếp
            else
                return null;
        }
    }
}