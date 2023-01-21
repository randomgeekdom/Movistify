namespace Movistify.Models
{
    public class Movie : Entity
    {
        public string Title { get; set; } = Constants.DefaultMovieTitle;
        public string Tagline { get; set; } = string.Empty;
    }
}
