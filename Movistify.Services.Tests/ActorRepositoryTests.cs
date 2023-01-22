using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Movistify.Dtos;
using Movistify.Models;

namespace Movistify.Services.Tests
{
    public class ActorRepositoryTests
    {
        private readonly ActorRepository sut;
        private Mock<MovistifyContext> mockContext;
        private Mock<IMapper> mockMapper;

        public ActorRepositoryTests()
        {
            this.mockContext = TestResources.GetMockContext();
            var mockcontextFactory = new Mock<IDbContextFactory<MovistifyContext>>();
            mockcontextFactory.Setup(x => x.CreateDbContext()).Returns(mockContext.Object);
            this.mockMapper = new Mock<IMapper>();

            this.sut = new ActorRepository(mockcontextFactory.Object, mockMapper.Object);
        }

        [Fact]
        public async Task WhenSearchingForExistingActorThenReturnActor()
        {

            this.mockMapper.Setup(x => x.Map<IEnumerable<ActorDto>>(It.Is<IEnumerable<Actor>>(x => x.Any()))).Returns(new List<ActorDto>
            {
                new ActorDto
                {
                    Name = "Morgan Freeman"
                }
            });
            var actors = await sut.SearchActorsAsync("Morgan");
            Assert.NotEmpty(actors);
        }

        [Fact]
        public async Task WhenSearchingForNonExistingActorThenReturnNothing()
        {
            var actors = await sut.SearchActorsAsync("Howard");
            Assert.Empty(actors);
        }

        [Fact]
        public async Task WhenAddingActorThenActorAdded()
        {
            var dto = new EditActorDto
            {
                Birthday = DateTime.Now,
                Name = "Test Person"
            };

            await sut.AddActorAsync(dto);

            this.mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
            this.mockMapper.Verify(x => x.Map<Actor>(dto));
        }

        [Fact]
        public async Task WhenGettingByIdThatExistsThenReturn()
        {
            var dto = new ActorDetailsDto
            {
                Name = "Morgan Freeman"
            };
            this.mockMapper.Setup(x => x.Map<ActorDetailsDto>(It.IsAny<Actor>())).Returns(dto);

            var id = new Guid("6f7a3cd5-b5a9-40b2-9bec-1d7c3cf5fd5a");

            var actor = await sut.GetByIdAsync(id);

            Assert.Equal(dto, actor);
        }

        [Fact]
        public async Task WhenGettingByIdThatDoesNotExistThenReturnNothing()
        {
            var id = new Guid("000000d5-b5a9-40b2-9bec-1d7c3cfdfd5a");

            var movie = await sut.GetByIdAsync(id);
            Assert.Null(movie);
        }

        [Fact]
        public async Task WhenUpdatingActorThatExistsThenUpdate()
        {
            var dto = new EditActorDto
            {
                Name = "Morgan Freeman",
                Birthday = DateTime.Now
            };

            var id = new Guid("6f7a3cd5-b5a9-40b2-9bec-1d7c3cf5fd5a");

            var success = await sut.UpdateActorAsync(id, dto);

            Assert.True(success);
        }

        [Fact]
        public async Task WhenUpdatingActorThatDoesNotExistDoNotUpdate()
        {
            var id = new Guid("000000d5-b5a9-40b2-9bec-1d7c3cfdfd5a");

            var success = await sut.UpdateActorAsync(id, new EditActorDto());
            Assert.False(success);
        }
    }
}