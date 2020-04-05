using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class ProductFinder : Finder<Product>, IProductFinder
    {
        public ProductFinder(DbSet<Product> entity) : base(entity)
        {
        }

        public Product GetById(int id)
        {
            return AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return AsQueryable().ToList();
        }

        public bool IsProductExists(Product product)
        {
            if (product == null) return false;
            return AsQueryable().Any(x => x.Id == product.Id);
        }
    }
}
