using Microsoft.AspNetCore.Mvc;
using RamenGo.API.Models;
using RamenGo.API.Repositories;

namespace RamenGo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrothsController : ControllerBase
    {
        private readonly IRepository<Broth> _repository;

        public BrothsController(IRepository<Broth> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Broth>>> GetAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("id")]
        public async Task<ActionResult<Broth>> GetAsync(string id)
        {
            Broth? broth = await _repository.GetAsync(id);

            if (broth is null)
            {
                ErrorResponse error = new()
                {
                    Error = "No broth with the provided id has been found."
                };

                return NotFound(error);
            }

            return Ok(broth);
        }
    }
}