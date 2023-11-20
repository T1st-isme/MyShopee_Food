using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shopee_Food.Controllers
{
    public class VoucherController : Controller
    {
         private readonly Shopee_Food.MongoDBContext.MongoDBContext _mongoDBContext;


        public VoucherController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            string databaseName = "ShopeeFood"; // Thay thế bằng tên database thực tế
            _mongoDBContext = new Shopee_Food.MongoDBContext.MongoDBContext(connectionString, databaseName);
        }

        public ActionResult Index()
        {
            var collection = _mongoDBContext.GetCollection<Voucher>("Voucher");

            // Retrieve all vouchers from the MongoDB collection
            var vouchers = collection.Find(FilterDefinition<Voucher>.Empty).ToList();

            // Pass the vouchers to the view
            return View(vouchers);
        }

        //[HttpPost]
        //public ActionResult Create()
        //{
        //    try
        //    {
        //        var collection = _mongoDBContext.GetCollection<Voucher>("Voucher");

        //        // Insert the new voucher into the MongoDB collection
        //        collection.InsertOne(voucher);

        //        // Redirect to the Index action after creating a voucher
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions or log errors
        //        return View();
        //    }
        //}
    }
}