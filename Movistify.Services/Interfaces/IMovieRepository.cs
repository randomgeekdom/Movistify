using Movistify.Dtos;

namespace Movistify.Services.Interfaces
{
    public interface IMovieRepository
    {
        Task<bool> AddActorToMovieAsync(Guid actorId, Guid movieId);
        Task AddMovieAsync(EditMovieDto newMovie);
        Task<bool> DeleteMovieAsync(Guid id);
        Task<MovieDetailsDto?> GetByIdAsync(Guid id);
        Task<bool> RateMovieAsync(MovieRatingDto ratingDto);
        Task<IEnumerable<MovieDto>> SearchMoviesAsync(string titleSearchTerm);
        Task<bool> UpdateMovieAsync(Guid id, EditMovieDto editMovieDto);
    }
}