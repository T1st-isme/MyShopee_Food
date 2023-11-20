using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Models
{
    public class Voucher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("NameVoucher")]
        public string NameVoucher { get; set; }

        [BsonElement("Discount")]
        public double Discount { get; set; }
    }
}