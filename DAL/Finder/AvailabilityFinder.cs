using BLL.Entities;
using BLL.Finders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Finder
{
    public class AvailabilityFinder : Finder<Availability>, IAvailabilityFinder
    {
        public AvailabilityFinder(DbSet<Availability> entity) : base(entity)
        {
        }

        public async Task<List<Availability>> GetAvailabilites()
        {
            return await AsQueryable().Include(y => y.Variant).ThenInclude(z => z.Product).ToListAsync();
        }

        public Availability GetAvailabilityById(int id)
        {
            return AsQueryable().Where(x => x.Id == id).Include(y => y.Variant).ThenInclude(z => z.Product).FirstOrDefault();
        }

        public Availability GetAvailabilityByVariantId(int id, int size)
        {
            return AsQueryable().Where(x => x.VariantId == id && x.Size == size).Include(y => y.Variant).ThenInclude(z => z.Product).FirstOrDefault();
        }
    }
}
