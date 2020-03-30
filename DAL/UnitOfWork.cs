using System;
using System.Collections.Generic;
using System.Text;
using BLL.DataAccess;
using DAL.Context;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
