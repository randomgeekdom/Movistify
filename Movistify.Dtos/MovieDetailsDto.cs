namespace Movistify.Dtos
{
    public class MovieDetailsDto:MovieDto
    {
        public IEnumerable<ActorDto> Actors { get; set; } = Enumerable.Empty<ActorDto>();
        public IEnumerable<MovieRatingDto> Ratings { get; set; } = Enumerable.Empty<MovieRatingDto>();
    }
}
