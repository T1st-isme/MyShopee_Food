using MongoDB.Driver;
using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.MongoDBContext
{
    public class MongoDBContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public MongoDBContext(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<Voucher> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<Voucher>(collectionName);
        }
    }
}