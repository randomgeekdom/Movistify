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

        public async Task<MovieDetailsDto?> GetByIdAsync(Guid id)
        {
            var movie = await this.movistifyContext.Movies.FirstOrDefaultAsync(x=>x.Id == id);

            if(movie != null)
            {
                var movieDto = this.mapper.Map<MovieDetailsDto>(movie);

                var actorIds = await this.movistifyContext.ActorMovies.Where(x => x.MovieId == id).Select(x => x.ActorId).ToListAsync();
                if (actorIds.Any())
                {
                    var actors = await this.movistifyContext.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
                    movieDto.Actors = this.mapper.Map<IEnumerable<ActorDto>>(actors);
                }

                var ratings = await this.movistifyContext.MovieRatings.Where(x => x.MovieId == id).ToListAsync();
                movieDto.Ratings = this.mapper.Map<IEnumerable<MovieRatingDto>>(ratings);

                return movieDto;
            }

            return null;
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

        public async Task<bool> RateMovieAsync(MovieRatingDto ratingDto)
        {
            if(ratingDto.Rating < 0 || ratingDto.Rating > 100 || string.IsNullOrWhiteSpace(ratingDto.ReviewerName))
            {
                return false;
            }

            var exists = await this.movistifyContext.Movies.AnyAsync(x => x.Id == ratingDto.MovieId);
            if (exists)
            {
                var rating = this.mapper.Map<MovieRating>(ratingDto);
                await this.movistifyContext.MovieRatings.AddAsync(rating);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
