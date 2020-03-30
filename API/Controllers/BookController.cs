using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Requests;
using AutoMapper;
using BLL.Entities;
using BLL.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("Books")]
    [EnableCors("Policy")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IUserManager userManager, IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _userManager = userManager;
            _logger = logger;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<Book[]> GetBooks()
        {
            var user = await GetActualUser();
            _logger.LogTrace($"Getting {user.Email} books...");
            IEnumerable<Book> userBookList = _bookService.GetAll(user.Id);
            Book[] books = userBookList.ToArray();
            _logger.LogTrace($"{user.Email} books was successfully found");
            return books;
        }

        public async Task<User> GetActualUser()
        {
            _logger.LogTrace("Getting actual User...");
            var identity = (ClaimsIdentity)this.User.Identity;
            var userEmail = identity.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            var user = await _userManager.GetUserByEmail(userEmail);
            _logger.LogTrace($"actual User is {user.Email}");
            return user;
        }

        [Route("Get")]
        [HttpGet("{id}")]
        public Book GetBook(int id)
        {
            return _bookService.GetBook(id);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<Book> UpdateBook(RequestBookModel book)
        {
            if (book != null)
            {
                var user = await GetActualUser();
                _logger.LogTrace($"{user.Email} trying to update {book.BookId}");

                Book actualBook = _bookService.GetBook(book.BookId);
                Mapper.Map(book, actualBook);
                _bookService.Update(actualBook);
                _logger.LogTrace($"{user.Email} successfully updated {book.BookId}");
                return actualBook;
            }

            return null;
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> AddBook(RequestBookModel book)
        {
            if (book.Content != null)
            {
                var user = await GetActualUser();
                _logger.LogTrace($"{user.Email} trying to Add new book");

                Book newBook = Mapper.Map<RequestBookModel, Book>(book);
                newBook.AuthorId = user.Id.ToString();
                newBook.ReleaseDate = DateTime.Now;
                _bookService.Create(newBook);

                _logger.LogTrace($"{newBook.Title} was created by {user.Email}");
                return Ok(newBook);
            }

            return BadRequest("Error");
        }

        [Route("Delete/{id}")]
        [HttpPost]
        public async Task<int> DeleteBook(int id)
        {
            var user = await GetActualUser();
            _logger.LogTrace($"Deleting Book by {user.Email}");
            var book = _bookService.GetBook(id);
            _bookService.Delete(book);
            _logger.LogTrace($"{book.Title} was deleted by {user.Email}");
            return id;
        }
    }
}