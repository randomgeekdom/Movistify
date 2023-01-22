using Microsoft.EntityFrameworkCore;
using Movistify.Models;

namespace Movistify
{
    public class MovistifyContext : DbContext
    {
        public MovistifyContext() { }
        public MovistifyContext(DbContextOptions<MovistifyContext> contextOptions) : base(contextOptions)
        {
        }

        
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieRating> MovieRatings { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<ActorMovie> ActorMovies { get; set; }
    }
}