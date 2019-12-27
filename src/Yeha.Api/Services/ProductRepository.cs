using System.Collections.Generic;
using System.Linq;
using Yeha.Api.Contracts;
using Yeha.Api.Models;

namespace Yeha.Api.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<IProduct> _products = new List<IProduct>();

        public void Add(IProduct product)
        {
            var exists = Exists(product.Id);
            if (exists) throw new System.ArgumentException($"The Product already exists. ");

            _products.Add(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Select(p => (Product) p);
        }

        private bool Exists(string id)
        {
            var match = _products.FirstOrDefault(f => f.Id == id);
            return match != null;
        }
    }
}
