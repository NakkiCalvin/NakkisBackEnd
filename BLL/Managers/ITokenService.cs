using BLL.Entities;
using System.Collections.Generic;

namespace BLL.Managers
{
    public interface ITokenService
    {
        string GetEncodedJwtToken(IList<string> userRoles, string userEmail);
    }
}