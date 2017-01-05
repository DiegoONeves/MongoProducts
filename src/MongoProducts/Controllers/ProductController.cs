using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoProducts.Interfaces;
using MongoProducts.Models;
using MongoProducts.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MongoProducts.Controllers
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController()
        {
            _productRepository = new ProductRepository(new MongoClient("mongodb://localhost:27017"));
        }

        [HttpPost]
        public IActionResult Post([FromBody]ProductViewModel productViewModel)
        {
            var product = new Product
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Category = productViewModel.Category
            };
            _productRepository.Add(product);

            productViewModel.ProductId = product._id.ToString();

            return new ObjectResult(productViewModel);
        }

        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            var product = _productRepository.GetById(new ObjectId(id));
            if (product == null)
            {
                return NotFound();
            }
            return new ObjectResult(ConvertProducts(product).FirstOrDefault());
        }

        public IActionResult Get()
        {
            var products = _productRepository.GetAll();
            if (products == null)
            {
                return NotFound();
            }

            return new ObjectResult(ConvertProducts(products.ToArray()));
        }

        [HttpPut]
        public IActionResult Put([FromBody]ProductViewModel productViewModel)
        {
            var recId = new ObjectId(productViewModel.ProductId);
            var productDb = _productRepository.GetById(new ObjectId(productViewModel.ProductId));

            if (productDb == null)
            {
                return NotFound();
            }

            productDb.Name = productViewModel.Name;
            productDb.Category = productViewModel.Category;
            productDb.Price = productViewModel.Price;

            _productRepository.Update(recId, productDb);
            return new ObjectResult(productViewModel);
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var product = _productRepository.GetById(new ObjectId(id));
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Remove(product._id);
            return new OkResult();
        }

        [NonAction]
        private IEnumerable<ProductViewModel> ConvertProducts(params Product[] products)
        {
            foreach (var item in products)
            {
                yield return new ProductViewModel { ProductId = item._id.ToString(), Name = item.Name, Category = item.Category, Price = item.Price };
            }
        }
    }
}
