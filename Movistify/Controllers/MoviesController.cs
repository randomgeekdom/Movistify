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

        /// <summary>
        /// Adds an actor to a movie
        /// </summary>
        /// <param name="actorId">The actor ID</param>
        /// <param name="movieId">The Movie Id</param>
        /// <returns>OK if the actor and the movie exist and if there is not an existing entry</returns>
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

        /// <summary>
        /// Deletes a movie from the database
        /// </summary>
        /// <param name="id">the movie ID</param>
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

        /// <summary>
        /// Gets a movie by ID including its ratings and actors
        /// </summary>
        /// <param name="id">Movie ID</param>
        /// <returns>A movie including its ratings and actors</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await this.movieRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Adds a new movie to the database
        /// </summary>
        /// <param name="movieDto">The movie's information</param>
        [HttpPost]
        public async Task<IActionResult> Post(EditMovieDto movieDto)
        {
            await this.movieRepository.AddMovieAsync(movieDto);
            return Ok();
        }

        /// <summary>
        /// Updates an existing movie in the database
        /// </summary>
        /// <param name="id">Movie Id</param>
        /// <param name="editMovieDto">The new information about the movie</param>
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

        /// <summary>
        /// Searches for movies based on a search term
        /// </summary>
        /// <param name="searchTerm">The term to search on</param>
        /// <returns>A list of movies where the title contains the search term</returns>
        [AllowAnonymous]
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var movies = await this.movieRepository.SearchMoviesAsync(searchTerm);
            return Ok(movies);
        }

        /// <summary>
        /// Adds a rating to a movie by a reviewer
        /// </summary>
        /// <param name="ratingDto">The rating information</param>
        [HttpPost("rate")]
        public async Task<IActionResult> Rate([FromBody] MovieRatingDto ratingDto)
        {
            var isSuccess = await this.movieRepository.RateMovieAsync(ratingDto);
            if (!isSuccess)
            {
                return new BadRequestObjectResult(ratingDto);
            }
            return Ok();
        }
    }
}