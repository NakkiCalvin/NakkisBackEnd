using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DataAccess
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Create(IEnumerable<T> entity);
        void Update(IEnumerable<T> entity);
        void Delete(IEnumerable<T> entity);
    }
}
