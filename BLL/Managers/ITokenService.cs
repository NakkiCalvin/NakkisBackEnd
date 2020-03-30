namespace BLL.Managers
{
    public interface ITokenService
    {
        string GetEncodedJwtToken(string userEmail);
    }
}