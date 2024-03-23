using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Pattern.ThanhToanStrategy
{
    public class Context
    {
        private IChoice _choice;

        public Context(IChoice choice)
        {
            _choice = choice;
        }

        public void SetChoice(IChoice choice)
        {
            _choice = choice;
        }

        public void Choice(HttpContextBase context, string DiaDiemGiaoHang, List<MatHangMua> cart)
        {
            _choice.Choice(context, DiaDiemGiaoHang, cart);
        }
    }
}