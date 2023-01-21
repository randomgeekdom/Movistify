namespace Movistify.Dtos
{
    public class ActorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public IEnumerable<MovieDto> Movies { get; set; } = Enumerable.Empty<MovieDto>();
    }
}
