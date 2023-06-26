using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Concrete
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;

        public BrandService(IGenericRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddAsync(Brand entity)
        {
            await _brandRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Brand entity)
        {
            await _brandRepository.DeleteAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _brandRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _brandRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return new Brand();
        }

        public IEnumerable<Brand> GetListByExpressionAsync(Func<Brand, bool> expression)
        {
            return _brandRepository.GetListByExpressionAsync(expression);
        }

        public async Task UpdateAsync(Brand entity)
        {
            await _brandRepository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}
