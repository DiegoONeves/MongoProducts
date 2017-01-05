using MongoDB.Bson;

namespace MongoProducts.Models
{
    public class Product
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
