using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Concrete
{
    public class CartItemService : ICartItemService
    {
        private readonly IGenericRepository<CartItem> _cartItemRepository;

        public CartItemService(IGenericRepository<CartItem> cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task AddAsync(CartItem entity)
        {
            await _cartItemRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task DeleteAsync(CartItem entity)
        {
            await _cartItemRepository.DeleteAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync()
        {
            return await _cartItemRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _cartItemRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<CartItem?> GetByIdAsync(Guid id)
        {
            return new CartItem();
        }

        public IEnumerable<CartItem> GetListByExpressionAsync(Func<CartItem, bool> expression)
        {
            return _cartItemRepository.GetListByExpressionAsync(expression);
        }

        public async Task UpdateAsync(CartItem entity)
        {
            await _cartItemRepository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}
