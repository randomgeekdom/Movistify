using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movistify.Dtos;
using Movistify.Models;
using Movistify.Services.Interfaces;

namespace Movistify.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbContextFactory<MovistifyContext> contextFactory;
        private readonly IMapper mapper;

        public MovieRepository(IDbContextFactory<MovistifyContext> contextFactory, IMapper mapper)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> SearchMoviesAsync(string titleSearchTerm)
        {
            var context = this.contextFactory.CreateDbContext();
            var results = await context.Movies.Where(x => x.Title.ToLower().Contains(titleSearchTerm.ToLower())).ToListAsync();
            return this.mapper.Map<IEnumerable<MovieDto>>(results);
        }

        public async Task AddMovieAsync(EditMovieDto newMovie)
        {
            var context = this.contextFactory.CreateDbContext();
            var movieEntity = this.mapper.Map<Movie>(newMovie);
            await context.Movies.AddAsync(movieEntity);
            await context.SaveChangesAsync();
        }

        public async Task<MovieDetailsDto?> GetByIdAsync(Guid id)
        {
            var context = this.contextFactory.CreateDbContext();
            var movie = await context.Movies.FirstOrDefaultAsync(x=>x.Id == id);

            if(movie != null)
            {
                var movieDto = this.mapper.Map<MovieDetailsDto>(movie);

                var actorIds = await context.ActorMovies.Where(x => x.MovieId == id).Select(x => x.ActorId).ToListAsync();
                if (actorIds.Any())
                {
                    var actors = await context.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
                    movieDto.Actors = this.mapper.Map<IEnumerable<ActorDto>>(actors);
                }

                var ratings = await context.MovieRatings.Where(x => x.MovieId == id).ToListAsync();
                movieDto.Ratings = this.mapper.Map<IEnumerable<MovieRatingDto>>(ratings);

                return movieDto;
            }

            return null;
        }

        public async Task<bool> UpdateMovieAsync(Guid id, EditMovieDto editMovieDto)
        {
            var context = this.contextFactory.CreateDbContext();
            var movie = await context.Movies.FindAsync(id);
            if (movie != null)
            {
                this.mapper.Map(editMovieDto, movie);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteMovieAsync(Guid id)
        {
            var context = this.contextFactory.CreateDbContext();
            var movie = await context.Movies.FindAsync(id);
            if (movie != null)
            {
                context.Remove(movie);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AddActorToMovieAsync(Guid actorId, Guid movieId)
        {
            var context = this.contextFactory.CreateDbContext();
            var exists = await context.ActorMovies.AnyAsync(x => x.MovieId == movieId && x.ActorId == actorId);
            if (!exists)
            {
                var movie = await context.Movies.FindAsync(movieId);
                var actor = await context.Actors.FindAsync(actorId);
                if (movie != null && actor != null)
                {
                    await context.ActorMovies.AddAsync(new ActorMovie
                    {
                        ActorId = actorId,
                        MovieId = movieId
                    });
                    await context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> RateMovieAsync(MovieRatingDto ratingDto)
        {
            var context = this.contextFactory.CreateDbContext();
            if (ratingDto.Rating < 0 || ratingDto.Rating > 100 || string.IsNullOrWhiteSpace(ratingDto.ReviewerName))
            {
                return false;
            }

            var exists = await context.Movies.AnyAsync(x => x.Id == ratingDto.MovieId);
            if (exists)
            {
                var rating = this.mapper.Map<MovieRating>(ratingDto);
                await context.MovieRatings.AddAsync(rating);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
