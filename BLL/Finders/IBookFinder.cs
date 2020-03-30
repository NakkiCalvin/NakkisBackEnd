using System;
using System.Collections.Generic;
using BLL.Entities;

namespace BLL.Finders
{
    public interface IBookFinder
    {
        Book GetById(int id);
        IEnumerable<Book> GetAll(Guid id);
        bool IsBookExists(Book book);
    }
}
