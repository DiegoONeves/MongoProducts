using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoProducts.Interfaces;

namespace MongoProducts.Models
{
    public class ProductRepository : IProductRepository
    {
        IMongoClient _client;
        IMongoDatabase _db;
        IMongoCollection<Product> _documents;
        public ProductRepository(IMongoClient client)
        {
            _client = client;
            _db = _client.GetDatabase("EmployeeDB");
            _documents = _db.GetCollection<Product>("Products");
        }
        public Product Add(Product document)
        {
            _documents.InsertOne(document);
            return document;
        }

        public IEnumerable<Product> GetAll()
        {
            return _documents.Find(_ => true).ToList();
        }

        public Product GetById(ObjectId id)
        {
            return _documents.Find(x => x._id == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetByCategory(string category)
        {
            return _documents.Find(x => x.Category == category).ToList();
        }

        public void Remove(ObjectId id)
        {
            _documents.DeleteOne(c => c._id == id);
        }

        public Product Update(ObjectId id, Product document)
        {
            _documents.ReplaceOne(x => x._id == id, document);
            return document;
        }

    }
}
