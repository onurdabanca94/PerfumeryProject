using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(User entity)
        {
            await _userRepository.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task DeleteAsync(User entity)
        {
            await _userRepository.DeleteAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public IEnumerable<User> GetListByExpressionAsync(Func<User, bool> expression)
        {
            return _userRepository.GetListByExpressionAsync(expression);
        }

        public async Task UpdateAsync(User entity)
        {
            await _userRepository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}
