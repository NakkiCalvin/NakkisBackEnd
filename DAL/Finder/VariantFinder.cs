using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Finder
{
    public class VariantFinder : Finder<Variant>, IVariantFinder
    {
        public VariantFinder(DbSet<Variant> entity) : base(entity)
        {
        }

        public IEnumerable<Variant> GetAllById(int productId)
        {
            return AsQueryable().Include(y => y.Availabilities).Include(x => x.Product).Where(_ => _.ProductId == productId).ToList();
        }
    }
}
