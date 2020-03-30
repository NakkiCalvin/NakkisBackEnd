using System;

namespace BLL.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
       // public virtual Author Author { get; set; }
    }
}
