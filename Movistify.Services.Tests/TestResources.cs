using Moq;
using Moq.EntityFrameworkCore;
using Movistify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movistify.Services
{
    internal static class TestResources
    {
        internal static Mock<MovistifyContext> GetMockContext()
        {
            var mock = new Mock<MovistifyContext>();
            var movies = new List<Movie>() {
                new Movie
                {
                    Id = new Guid("bb213b2e-f73c-4ba7-83f7-426c97c26996"),
                    Tagline = "A prison Movie",
                    Title = "The Shawshank Redemption",
                    Year = 1994
                }
            };
            var actors = new List<Actor>() {
                new Actor
                {
                    Id = new Guid("6f7a3cd5-b5a9-40b2-9bec-1d7c3cf5fd5a"),
                    Name = "Morgan Freeman",
                    Birthday = new DateTime(1937, 6, 1)
                }
            };
            var actorMovies = new List<ActorMovie>() {
                new ActorMovie
                {
                    Id = new Guid("f3b66428-7ea2-4a43-8fd6-c376c9085256"),
                    MovieId = new Guid("bb213b2e-f73c-4ba7-83f7-426c97c26996"),
                    ActorId = new Guid("6f7a3cd5-b5a9-40b2-9bec-1d7c3cf5fd5a"),
                }
            };
            var movieRatings = new List<MovieRating>() {
                new MovieRating
                {
                    Id = new Guid("754c503a-569d-48c8-b3e9-6203da423fc4"),
                    Rating = 100,
                    ReviewerName = "Gene Siskel"
                }
            };

            mock.Setup(x => x.Movies).ReturnsDbSet(movies);
            mock.Setup(x => x.Actors).ReturnsDbSet(actors);
            mock.Setup(x => x.ActorMovies).ReturnsDbSet(actorMovies);
            mock.Setup(x => x.MovieRatings).ReturnsDbSet(movieRatings);


            return mock;
        }
    }
}
