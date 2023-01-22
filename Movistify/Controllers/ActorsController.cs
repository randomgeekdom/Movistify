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


        /// <summary>
        /// Deletes an actor from the database by the ID
        /// </summary>
        /// <param name="id">Actor Id (Guid)</param>
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

        /// <summary>
        /// Get an actor and all their movies by ID
        /// </summary>
        /// <param name="id">Actor id</param>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await this.ActorRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Add a new actor in the database
        /// </summary>
        /// <param name="ActorDto">The actor's information</param>
        [HttpPost]
        public async Task<IActionResult> Post(EditActorDto ActorDto)
        {
            await this.ActorRepository.AddActorAsync(ActorDto);
            return Ok();
        }

        /// <summary>
        /// Update an existing actor's information
        /// </summary>
        /// <param name="id">actor id</param>
        /// <param name="editActorDto">The new information about the actor</param>
        /// <returns>OK if the actor exists.  Bad Request if not.</returns>
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

        /// <summary>
        /// Search for an actor by their name
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>A list of actors with names that contain the search term</returns>
        [AllowAnonymous]
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var Actors = await this.ActorRepository.SearchActorsAsync(searchTerm);
            return Ok(Actors);
        }
    }
}