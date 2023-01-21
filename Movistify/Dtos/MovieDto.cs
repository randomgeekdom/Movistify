namespace Movistify.Dtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public long Year { get; set; }
        public IEnumerable<ActorDto> Actors { get; set; } = Enumerable.Empty<ActorDto>();
    }
}
