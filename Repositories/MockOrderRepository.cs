using RamenGo.API.Models;

namespace RamenGo.API.Repositories
{
    public class MockOrderRepository : IRepository<Order>
    {
        private readonly List<Order> _orders;

        public MockOrderRepository()
        {
            _orders = new List<Order>();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task<Order?> GetAsync(string id)
        {
            return await Task.FromResult(_orders.FirstOrDefault(order => order.Id == id));
        }

        public async Task AddAsync(Order item)
        {
            _orders.Add(item);

            await Task.CompletedTask;
        }
    }
}