using Microsoft.AspNetCore.Mvc;
using RamenGo.API.Models;
using RamenGo.API.Repositories;
using RamenGo.API.Services;

namespace RamenGo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IOrdersService _service;

        public OrdersController(IRepository<Order> repository, IOrdersService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("id")]
        public async Task<ActionResult<Order>> GetAsync(string id)
        {
            Order? order = await _repository.GetAsync(id);

            if (order is null)
            {
                ErrorResponse error = new()
                {
                    Error = "No order with the provided id has been found."
                };

                return NotFound(error);
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateAsync([FromHeader(Name = "x-api-key")] string apiKey, [FromBody] OrderRequest request)
        {
            OrderResponse? response = await _service.CreateOrderAsync(apiKey, request);

            if (response is null)
            {
                ErrorResponse error = new()
                {
                    Error = "Could not place order."
                };

                return BadRequest(error);
            }

            return response;
        }
    }
}