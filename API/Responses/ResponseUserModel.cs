using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class ResponseUserModel
    {
        public ResponseUserModel() { }

        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
