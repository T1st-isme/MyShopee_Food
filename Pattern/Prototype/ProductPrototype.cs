using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Models
{
    public abstract class ProductPrototype
    {
        public abstract ProductPrototype Clone();
    }
}