using System;
using System.Collections.Generic;
using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Book> _repository;
        readonly IBookFinder _finder;

        public BookService(IUnitOfWork unitOfWork, IRepository<Book> repository, IBookFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public Book GetBook(int id)
        {
            return _finder.GetById(id);
        }

        public IEnumerable<Book> GetAll(Guid id)
        {
            return _finder.GetAll(id);
        }

        public void Create(Book book)
        {
            if(book == null) return;
            _repository.Create(book);
            _unitOfWork.Commit();
        }

        public void Update(Book book)
        {
            if (book == null) return;
            _repository.Update(book);
            _unitOfWork.Commit();
        }

        public void Delete(Book book)
        {
            if (book == null) return;
            _repository.Delete(book);
            _unitOfWork.Commit();
        }
    }
}
