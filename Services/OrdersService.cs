using Microsoft.Extensions.Options;
using RamenGo.API.Models;
using RamenGo.API.Repositories;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RamenGo.API.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppConfig _appConfig;
        private HttpClient _client = new();

        private readonly IRepository<Broth> _brothRepository;
        private readonly IRepository<Protein> _proteinRepository;
        private readonly IRepository<Order> _orderRepository;

        public OrdersService(IOptions<AppConfig> options,
                            IRepository<Broth> brothRepository,
                            IRepository<Protein> proteinRepository,
                            IRepository<Order> orderRepository)
        {
            _appConfig = options.Value;

            _brothRepository = brothRepository;
            _proteinRepository = proteinRepository;
            _orderRepository = orderRepository;

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("x-api-key", _appConfig.ApiKey);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<OrderResponse?> CreateOrderAsync(string apiKey, OrderRequest orderRequest)
        {
            if (apiKey != _appConfig.ApiKey)
            {
                return null;
            }

            using HttpRequestMessage request = new(HttpMethod.Post, new Uri("https://api.tech.redventures.com.br/orders/generate-id"));

            using StringContent content = new(string.Empty, Encoding.UTF8, "application/json");

            request.Content = content;

            using HttpResponseMessage response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            OrderIdCreatedResponse? orderIdResponse = JsonSerializer.Deserialize<OrderIdCreatedResponse>(responseBody, options);

            if (orderIdResponse == null || orderIdResponse.OrderId == string.Empty)
            {
                return null;
            };

            string description = await GetDescriptionAsync(orderRequest.BrothId, orderRequest.ProteinId);

            if (description == string.Empty)
            {
                return null;
            }

            string image = await GetImageAsync(orderRequest.ProteinId);

            Order order = new()
            {
                Id = orderIdResponse.OrderId,
                BrothId = orderRequest.BrothId,
                ProteinId = orderRequest.ProteinId,
                Description = description,
                Image = image
            };

            await _orderRepository.AddAsync(order);

            OrderResponse orderResponse = new()
            {
                Id = order.Id,
                Description = order.Description,
                Image = order.Image
            };

            return orderResponse;
        }

        private async Task<string> GetDescriptionAsync(string brothId, string proteinId)
        {
            Task<Broth?> brothTask = _brothRepository.GetAsync(brothId);
            Task<Protein?> proteinTask = _proteinRepository.GetAsync(proteinId);

            await Task.WhenAll(brothTask, proteinTask);

            Broth? broth = await brothTask;
            Protein? protein = await proteinTask;

            if (broth == null || protein == null)
            {
                return string.Empty;
            }

            return $"{broth.Name} and {protein.Name} Ramen";
        }

        private async Task<string> GetImageAsync(string proteinId)
        {
            const string BASEURL = "https://tech.redventures.com.br/icons/ramen/";

            Protein? protein = await _proteinRepository.GetAsync(proteinId);

            if (protein == null)
            {
                return string.Empty;
            }

            return protein.Name switch
            {
                "Chasu" => $"{BASEURL}ramenChasu.png",
                "Yasai Vegetarian" => $"{BASEURL}ramenYasai Vegetarian.png",
                "Karaague" => $"{BASEURL}ramenKaraague.png",
                _ => string.Empty,
            };
        }
    }
}