using RamenGo.API.Models;

namespace RamenGo.API.Services
{
    public interface IOrdersService
    {
        Task<OrderResponse?> CreateOrderAsync(string apiKey, OrderRequest request);
    }
}