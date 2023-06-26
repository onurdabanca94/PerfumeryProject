using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Concrete
{
    public class ParfumService : IParfumService
    {
        private readonly IGenericRepository<Parfum> _parfumRepository;

        public ParfumService(IGenericRepository<Parfum> parfumRepository)
        {
            _parfumRepository = parfumRepository;
        }

        public async Task AddAsync(Parfum entity)
        {
            await _parfumRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Parfum entity)
        {
            await _parfumRepository.DeleteAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Parfum>> GetAllAsync()
        {
            return await _parfumRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Parfum?> GetByIdAsync(int id)
        {
            return await _parfumRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Parfum?> GetByIdAsync(Guid id)
        {
            return new Parfum();
        }

        public IEnumerable<Parfum> GetListByExpressionAsync(Func<Parfum, bool> expression)
        {
            return _parfumRepository.GetListByExpressionAsync(expression);
        }

        public async Task UpdateAsync(Parfum entity)
        {
            await _parfumRepository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}
