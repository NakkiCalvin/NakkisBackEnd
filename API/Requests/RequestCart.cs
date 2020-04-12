using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Requests
{
    public class RequestCart
    {
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public bool increase { get; set; }
        public bool decrease { get; set; }
    }
}
