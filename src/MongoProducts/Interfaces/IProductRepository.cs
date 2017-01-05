using MongoDB.Bson;
using System.Collections.Generic;
using MongoProducts.Models;

namespace MongoProducts.Interfaces
{
    public interface IProductRepository
    {
        Product Add(Product document);
        Product Update(ObjectId id, Product document);
        IEnumerable<Product> GetAll();
        Product GetById(ObjectId id);
        IEnumerable<Product> GetByCategory(string category);
        void Remove(ObjectId id);
    }
}
