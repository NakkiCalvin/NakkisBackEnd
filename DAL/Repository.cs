using System.Collections.Generic;
using BLL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _entity;

        public Repository(DbSet<T> entity)
        {
            this._entity = entity;
        }

        public void Create(T entity)
        {
            this._entity.Add(entity);
        }

        public void Update(T entity)
        {
            this._entity.Update(entity);
        }

        public void Delete(T entity)
        {
            this._entity.Remove(entity);
        }

        public void Create(IEnumerable<T> entity)
        {
            this._entity.AddRange(entity);
        }

        public void Update(IEnumerable<T> entity)
        {
            this._entity.UpdateRange(entity);
        }

        public void Delete(IEnumerable<T> entity)
        {
            this._entity.RemoveRange(entity);
        }
    }
}
