using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Order> _repository;
        readonly IOrderFinder _finder;

        public OrderService(IUnitOfWork unitOfWork, IRepository<Order> repository, IOrderFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public Order GetOrderByUserId(Guid userId)
        {
            return _finder.GetOrderByUserId(userId);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _finder.GetOrderById(orderId);
        }

        public void Create(Order order)
        {
            if (order == null) return;
            _repository.Create(order);
            _unitOfWork.Commit();
        }

        public void Update(Order order)
        {
            if (order == null) return;
            _repository.Update(order);
            _unitOfWork.Commit();
        }
    }
}
