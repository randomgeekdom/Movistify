namespace Movistify.Dtos
{
    public class MovieRatingDto
    {
        public Guid MovieId { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; }
    }
}
