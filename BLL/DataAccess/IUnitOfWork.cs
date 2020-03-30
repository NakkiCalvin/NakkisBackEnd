using BLL.Entities;

namespace BLL.DataAccess
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
