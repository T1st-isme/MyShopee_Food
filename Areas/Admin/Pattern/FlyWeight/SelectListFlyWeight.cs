using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Pattern.FlyWeight
{
    public class SelectListFlyWeight
    {
        private Dictionary<string, SelectList> _selectLists;

        public SelectListFlyWeight()
        {
            _selectLists = new Dictionary<string, SelectList>();
        }

        public SelectList GetSelectList(string key, IQueryable<object> dataSource, string dataValueField, string dataTextField, object selectedValue = null)
        {
            if (!_selectLists.ContainsKey(key))
            {
                _selectLists[key] = new SelectList(dataSource, dataValueField, dataTextField, selectedValue);
            }
            return _selectLists[key];
        }
    }
}