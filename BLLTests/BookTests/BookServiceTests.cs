using System;
using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Services;
using Moq;
using Xunit;

namespace BLLTests.BookTests
{
    public class BookServiceTests
    {
        static readonly Mock<IRepository<Book>> _reposMock = new Mock<IRepository<Book>>();
        static readonly Mock<IUnitOfWork> _unitMock = new Mock<IUnitOfWork>();
        static readonly Mock<IBookFinder> _finderMock = new Mock<IBookFinder>();
        readonly BookService _bookService = new BookService(_unitMock.Object, _reposMock.Object, _finderMock.Object);

        static Book book = new Book { BookId = 3, Title = "Cake3", ReleaseDate = new DateTime(2009, 01, 03) };

        [Fact]
        public void MoqCheckCreate()
        {
            _bookService.Create(book);
            _reposMock.Verify(p => p.Create(It.IsAny<Book>()), Times.AtLeastOnce);
            _unitMock.Verify(p => p.Commit(), Times.Once);
        }

        [Fact]
        public void MoqCheckNull()
        {
            book = null;
            _bookService.Create(book);
            _reposMock.Verify(p => p.Create(It.IsAny<Book>()), Times.Never);
            _unitMock.Verify(p => p.Commit(), Times.Never);
        }

        [Fact]
        public void MoqCheckRemove()
        {
            _bookService.Delete(book);
            _reposMock.Verify(p => p.Delete(book), Times.AtLeastOnce);
            _unitMock.Verify(p => p.Commit(), Times.Once);
        }

        [Fact]
        public void MoqCheckUpdate()
        {
            _bookService.Update(book);
            _reposMock.Verify(p => p.Update(book), Times.AtLeastOnce);
            _unitMock.Verify(p => p.Commit(), Times.Once);
        }

        [Fact]
        public void MoqCheckExist()
        {
            _finderMock.Setup(p => p.GetById(book.BookId)).Returns(new Book());
        }
    }
}
