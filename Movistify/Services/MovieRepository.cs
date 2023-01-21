using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movistify.Dtos;
using Movistify.Models;
using Movistify.Services.Interfaces;

namespace Movistify.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovistifyContext movistifyContext;
        private readonly IMapper mapper;

        public MovieRepository(MovistifyContext movistifyContext, IMapper mapper)
        {
            this.movistifyContext = movistifyContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> SearchMoviesAsync(string titleSearchTerm)
        {
            var results = await this.movistifyContext.Movies.Where(x => x.Title.ToLower().Contains(titleSearchTerm)).ToListAsync();
            return this.mapper.Map<IEnumerable<MovieDto>>(results);
        }

        public async Task AddMovieAsync(EditMovieDto newMovie)
        {
            var movieEntity = this.mapper.Map<Movie>(newMovie);
            await this.movistifyContext.Movies.AddAsync(movieEntity);
            await this.movistifyContext.SaveChangesAsync();
        }

        public async Task<MovieDto> GetByIdAsync(Guid id)
        {
            var movie = await this.movistifyContext.Movies.FindAsync(id);
            return this.mapper.Map<MovieDto>(movie);
        }

        public async Task<bool> UpdateMovieAsync(Guid id, EditMovieDto editMovieDto)
        {
            var movie = await this.movistifyContext.Movies.FindAsync(id);
            if (movie != null)
            {
                this.mapper.Map(editMovieDto, movie);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteMovieAsync(Guid id)
        {
            var movie = await this.movistifyContext.Movies.FindAsync(id);
            if (movie != null)
            {
                this.movistifyContext.Remove(movie);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AddActorToMovieAsync(Guid actorId, Guid movieId)
        {
            var exists = await this.movistifyContext.ActorMovies.AnyAsync(x => x.MovieId == movieId && x.ActorId == actorId);
            if (!exists)
            {
                var movie = await this.movistifyContext.Movies.FindAsync(movieId);
                var actor = await this.movistifyContext.Actors.FindAsync(actorId);
                if (movie != null && actor != null)
                {
                    await this.movistifyContext.ActorMovies.AddAsync(new ActorMovie
                    {
                        ActorId = actorId,
                        MovieId = movieId
                    });
                    await this.movistifyContext.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
    }
}
