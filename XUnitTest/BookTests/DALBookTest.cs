using System;
using System.Linq;
using BLL.Entities;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DALTest.BookTests
{
    public class DALBookTest
    {
        public static DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "BookAppDb")
            .Options;

        public ApplicationContext context = new ApplicationContext(options);

        [Fact]
        public void CheckWriteToBase()
        {
            context.Books.Add(new Book() { BookId = 1, Title = "Jungle", ReleaseDate = new DateTime(2005, 05, 02) });
            context.Books.Add(new Book() { BookId = 2, Title = "Cake", ReleaseDate = new DateTime(2005, 05, 02) });
            context.Books.Add(new Book() { BookId = 3, Title = "Smth", ReleaseDate = new DateTime(2005, 05, 02) });
            context.SaveChanges();
        
            Assert.Equal(3, context.Books.Count());
        }

        [Fact]
        public void CheckDataBase()
        {
            Assert.NotNull(context);
        }

        [Fact]
        public void CheckRemove()
        {
            var book = new Book { BookId = 6, Title = "Cake2", ReleaseDate = new DateTime(2009, 01, 03) };
            context.Books.Add(book);
            context.SaveChanges();
            context.Books.Remove(book);
            context.SaveChanges();

            Assert.Null(context.Books.Where(p => p.Title == "Cake2"));
        }

        [Fact]
        public void CheckUpdate()
        {
            var book = new Book { BookId = 6, Title = "Cake2", ReleaseDate = new DateTime(2009, 01, 03) };
            context.Books.Add(book);
            context.SaveChanges();
            book.Title = "Cake2";
            context.Books.Update(book);
            context.SaveChanges();

            Assert.Equal("Cake2", book.Title);
        }
    }
}
