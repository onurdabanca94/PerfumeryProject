using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Business.Abstraction
{
    public interface IOrderService : IGenericRepository<Order>
    {
    }
}
