namespace Movistify.Models
{
    public class MovieRating : Entity
    {
        public Guid MovieId { get; set; }

        // Rating from 0 to 100
        public int Rating { get; set; }

        // Name of reviewer who gave the rating
        public string ReviewerName { get; set; } = Constants.DefaultName;
    }
}
