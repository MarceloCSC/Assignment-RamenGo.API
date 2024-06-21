using Microsoft.AspNetCore.Mvc;
using RamenGo.API.Models;
using RamenGo.API.Repositories;
using RamenGo.API.Services;

namespace RamenGo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IOrderService _service;

        public OrderController(IRepository<Order> repository, IOrderService service)
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
        public async Task<ActionResult<OrderResponse>> CreateAsync(OrderRequest request)
        {
            OrderResponse? response = await _service.CreateOrderAsync(request);

            if (response is null)
            {
                ErrorResponse error = new()
                {
                    Error = "No order has been created."
                };

                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetAsync), new { response.Id }, response);
        }
    }
}