namespace Movistify.Models
{
    public class ActorMovie : Entity
    {
        public Guid ActorId { get; set; }
        public Guid MovieId { get; set; }
    }
}
