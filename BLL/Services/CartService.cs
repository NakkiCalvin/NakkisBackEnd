using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CartService : ICartService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Cart> _repository;
        readonly ICartFinder _finder;

        public CartService(IUnitOfWork unitOfWork, IRepository<Cart> repository, ICartFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public Cart GetCartByUserId(Guid userId)
        {
            return _finder.GetCartByUserId(userId);
        }

        public void Create(Cart cart)
        {
            if (cart == null) return;
            _repository.Create(cart);
            _unitOfWork.Commit();
        }

        public void Update(Cart cart)
        {
            if (cart == null) return;
            _repository.Update(cart);
            _unitOfWork.Commit();
        }
    }
}
