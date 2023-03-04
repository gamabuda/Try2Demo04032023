using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Try2Demo04032023.Model;

namespace Try2Demo04032023.Services
{
    public static class DataBaseManager
    {
        private static LopushokBDEntities _lopushokBDEntities = new LopushokBDEntities();

        public static List<Product> GetProducts() 
        {
            return _lopushokBDEntities.Product.ToList();
        }

        public static List<ProductType> GetProductTypes()
        {
            return _lopushokBDEntities.ProductType.ToList();
        }

        public static void AddProduct(Product product)
        {
            _lopushokBDEntities.Product.Add(product);
            _lopushokBDEntities.SaveChanges();
        }

        public static bool Try2SaveChanges()
        {
            try
            {
                _lopushokBDEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
