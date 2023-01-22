namespace Movistify.Dtos
{
    public class ActorDetailsDto : ActorDto
    {
        public IEnumerable<MovieDto> Movies { get; set; } = Enumerable.Empty<MovieDto>();
    }
}