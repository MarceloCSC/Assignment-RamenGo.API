using Microsoft.AspNetCore.Mvc;
using RamenGo.API.Models;
using RamenGo.API.Repositories;

namespace RamenGo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProteinsController : ControllerBase
    {
        private readonly IRepository<Protein> _repository;

        public ProteinsController(IRepository<Protein> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Protein>>> GetAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("id")]
        public async Task<ActionResult<Protein>> GetAsync(string id)
        {
            Protein? protein = await _repository.GetAsync(id);

            if (protein is null)
            {
                ErrorResponse error = new()
                {
                    Error = "No protein with the provided id has been found."
                };

                return NotFound(error);
            }

            return Ok(protein);
        }
    }
}