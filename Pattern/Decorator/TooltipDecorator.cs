using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food
{
    public class TooltipDecorator : SanPham
    {
        protected dynamic _component;

        public TooltipDecorator(dynamic component, int MaSP)
        {
            _component = component;
            _component.MaSP = MaSP;
        }

        public string MoTa
        {
            get
            {
                return $"{_component.MoTa}\n{_extraInfo}";
            }
            set
            {
                _component.MoTa = value;
            }
        }

        private string _extraInfo;

        public string ExtraInfo
        {
            get { return _extraInfo; }
            set { _extraInfo = value; }
        }
    }
}