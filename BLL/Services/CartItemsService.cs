using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CartItemsService : ICartItemsService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<CartItem> _repository;
        readonly ICartItemFinder _finder;

        public CartItemsService(IUnitOfWork unitOfWork, IRepository<CartItem> repository, ICartItemFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }

        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId)
        {
            return _finder.GetCartItemsByCartId(cartId);
        }

        public void Create(CartItem cartItem)
        {
            if (cartItem == null) return;
            _repository.Create(cartItem);
            _unitOfWork.Commit();
        }

        public void Update(CartItem cartItem)
        {
            if (cartItem == null) return;
            _repository.Update(cartItem);
            _unitOfWork.Commit();
        }

        public void Delete(CartItem cartItem)
        {
            if (cartItem == null) return;
            _repository.Delete(cartItem);
            _unitOfWork.Commit();
        }

        public void Delete(IEnumerable<CartItem> cartItems)
        {
            if (cartItems == null) return;
            _repository.Delete(cartItems);
            _unitOfWork.Commit();
        }
    }
}
