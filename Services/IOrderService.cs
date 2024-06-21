using RamenGo.API.Models;

namespace RamenGo.API.Services
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request);
    }
}