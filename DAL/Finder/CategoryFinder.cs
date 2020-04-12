using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class CategoryFinder : Finder<Category>, ICategoryFinder
    {
        public CategoryFinder(DbSet<Category> entity) : base(entity)
        {
        }

        public IEnumerable<Category> GetAll()
        {
            return AsQueryable().ToList();
        }
    }
}
