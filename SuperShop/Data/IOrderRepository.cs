using SuperShop.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetorderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(string userName);
    }
}
