using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movistify.Dtos;
using Movistify.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movistify.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorRepository ActorRepository;

        public ActorsController(IActorRepository ActorRepository)
        {
            this.ActorRepository = ActorRepository;
        }

        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isSuccess = await this.ActorRepository.DeleteActorAsync(id);
            if (!isSuccess)
            {
                return BadRequest($"Invalid ID: {id}");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await this.ActorRepository.GetByIdAsync(id));
        }

        // POST api/<ActorsController>
        [HttpPost]
        public async Task<IActionResult> Post(EditActorDto ActorDto)
        {
            await this.ActorRepository.AddActorAsync(ActorDto);
            return Ok();
        }

        // PUT api/<ActorsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EditActorDto editActorDto)
        {
            var isSuccess = await this.ActorRepository.UpdateActorAsync(id, editActorDto);
            if (!isSuccess)
            {
                return BadRequest($"Invalid ID: {id}");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var Actors = await this.ActorRepository.SearchActorsAsync(searchTerm);
            return Ok(Actors);
        }
    }
}