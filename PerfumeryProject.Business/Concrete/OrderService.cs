using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;

        public OrderService(IGenericRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddAsync(Order entity)
        {
            await _orderRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Order entity)
        {
            await _orderRepository.DeleteAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return new Order();
        }

        public IEnumerable<Order> GetListByExpressionAsync(Func<Order, bool> expression)
        {
            return _orderRepository.GetListByExpressionAsync(expression);
        }

        public async Task UpdateAsync(Order entity)
        {
            await _orderRepository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}
