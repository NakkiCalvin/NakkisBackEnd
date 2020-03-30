using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class ResponseBookModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
    }
}
