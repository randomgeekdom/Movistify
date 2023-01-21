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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpPost("actor")]
        public async Task<IActionResult> AddActorToMovie(Guid actorId, Guid movieId)
        {
            bool result = await this.movieRepository.AddActorToMovieAsync(actorId, movieId);
            if (result)
            {
                return BadRequest($"Unable to add actor to movie.  ActorId: {actorId}.  Movie Id: {movieId}.");
            }

            return Ok();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isSuccess = await this.movieRepository.DeleteMovieAsync(id);
            if (!isSuccess)
            {
                return BadRequest($"Invalid ID: {id}");
            }
            return Ok();
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await this.movieRepository.GetByIdAsync(id));
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<IActionResult> Post(EditMovieDto movieDto)
        {
            await this.movieRepository.AddMovieAsync(movieDto);
            return Ok();
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EditMovieDto editMovieDto)
        {
            var isSuccess = await this.movieRepository.UpdateMovieAsync(id, editMovieDto);
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
            var movies = await this.movieRepository.SearchMoviesAsync(searchTerm);
            return Ok(movies);
        }
    }
}