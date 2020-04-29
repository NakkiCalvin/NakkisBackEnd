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
            var product = AsQueryable().FirstOrDefault(x => x.Id == id);
            var allSizeProducts = AsQueryable().Where(_ => _.ImagePath == product.ImagePath && _.Id != product.Id).ToList();
            var productSizes = allSizeProducts.Select(x => x.Size);
            product.VariantSizes = productSizes;
                //sizes.Add(variant.ImagePath, productSizes);
            return product;
        }

        public IEnumerable<Product> GetAllProductClean()
        {
            return AsQueryable().Include(c => c.Department).Include(y => y.Category).ToList();
        }

        public IEnumerable<Product> GetVariantsByProductId(int productId)
        {
            var product = AsQueryable().FirstOrDefault(_ => _.Id == productId);
            var allSizeVariants = AsQueryable().Where(_ => _.Title == product.Title && _.ImagePath != product.ImagePath && _.Id != product.Id).ToList();
            var variants = allSizeVariants.GroupBy(x => x.ImagePath).Select(g => g.First()).ToList();
            //var sizes = new Dictionary<string, List<string>>();
            foreach (var variant in variants)
            {
                var productSizes = allSizeVariants.Where(y => y.ImagePath == variant.ImagePath).Select(x => x.Size);
                variant.VariantSizes = productSizes;
                //sizes.Add(variant.ImagePath, productSizes);
            }
            return variants;
        }

        public IEnumerable<Product> GetAll(string department, string category, string order, string range)
        {
            var result = AsQueryable().Include(c => c.Department).Include(y => y.Category).ToList();
            result = result.GroupBy(p => p.Title).Select(g => g.First()).ToList();

            if (category != null)
            {
                result = result.Where(_ => _.Category.CategoryName == category).ToList();
            }
            if (department != null)
            {
                result = result.Where(_ => _.Department.DepartmentName == department).ToList();
            }
            if (order != null)
            {
                if (order == "price")
                {
                    result = result.OrderBy(_ => _.Price).ToList();
                }
                if (order == "-price")
                {
                    result = result.OrderByDescending(_ => _.Price).ToList();
                }
            }
            if (range != null)
            {
                if (range == "0-29")
                {
                    result = result.Where(_ => _.Price <= 29).ToList();
                }
                if (range == "29-39")
                {
                    result = result.Where(_ => _.Price >= 29 && _.Price <= 39).ToList();
                }
                if (range == "39-49")
                {
                    result = result.Where(_ => _.Price >= 39 && _.Price <= 49).ToList();
                }
                if (range == "49-89")
                {
                    result = result.Where(_ => _.Price >= 49 && _.Price <= 89).ToList();
                }
                if (range == "89-999")
                {
                    result = result.Where(_ => _.Price >= 89 && _.Price <= 999).ToList();
                }
            }
            return result;
        }

        public bool IsProductExists(Product product)
        {
            if (product == null) return false;
            return AsQueryable().Any(x => x.Id == product.Id);
        }
    }
}
