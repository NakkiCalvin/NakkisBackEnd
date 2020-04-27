using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class OrderItemService : IOrderItemService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<OrderItem> _repository;
        readonly IOrderItemFinder _finder;

        public OrderItemService(IUnitOfWork unitOfWork, IRepository<OrderItem> repository, IOrderItemFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _finder.GetOrderItemsByOrderId(orderId);
        }

        public void Create(OrderItem orderItem)
        {
            if (orderItem == null) return;
            _repository.Create(orderItem);
            _unitOfWork.Commit();
        }

        public void Update(OrderItem orderItem)
        {
            if (orderItem == null) return;
            _repository.Update(orderItem);
            _unitOfWork.Commit();
        }

        public void Delete(OrderItem orderItem)
        {
            if (orderItem == null) return;
            _repository.Delete(orderItem);
            _unitOfWork.Commit();
        }
    }
}
