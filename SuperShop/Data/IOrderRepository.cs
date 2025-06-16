using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetorderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailsTempAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task ModifyOrderDetailTempQuantityAscyn(int id, double quantity);
    }
}
