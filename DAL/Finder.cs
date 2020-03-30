using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class Finder<T> where T : class
    {
        private readonly DbSet<T> _entity;

        public Finder(DbSet<T> entity)
        {
            _entity = entity;
        }

        protected IQueryable<T> AsQueryable()
        {
            return _entity.AsQueryable();
        }
    }
}
