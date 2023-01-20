using Microsoft.EntityFrameworkCore;
using Movistify.Models;

namespace Movistify
{
    public class MovistifyContext : DbContext
    {
        public MovistifyContext(DbContextOptions<MovistifyContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }
    }
}