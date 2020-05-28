using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class AvalabilityModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public int Quantity { get; set; }
    }
}
