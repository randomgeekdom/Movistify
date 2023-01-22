using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movistify.Dtos;
using Movistify.Models;
using Movistify.Services.Interfaces;

namespace Movistify.Services
{
    public class ActorRepository : IActorRepository
    {
        private readonly IDbContextFactory<MovistifyContext> contextFactory;
        private readonly IMapper mapper;

        public ActorRepository(IDbContextFactory<MovistifyContext> contextFactory, IMapper mapper)
        {
            this.contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ActorDto>> SearchActorsAsync(string nameSearchTerm)
        {
            var context = this.contextFactory.CreateDbContext();
            var results = await context.Actors.Where(x => x.Name.ToLower().Contains(nameSearchTerm.ToLower())).ToListAsync();
            return this.mapper.Map<IEnumerable<ActorDto>>(results);
        }

        public async Task AddActorAsync(EditActorDto newActor)
        {
            var context = this.contextFactory.CreateDbContext();
            var actorEntity = this.mapper.Map<Actor>(newActor);
            await context.Actors.AddAsync(actorEntity);
            await context.SaveChangesAsync();
        }

        public async Task<ActorDetailsDto?> GetByIdAsync(Guid id)
        {
            var context = this.contextFactory.CreateDbContext();
            var actor = await context.Actors.FirstOrDefaultAsync(x=>x.Id == id);

            var dto = default(ActorDetailsDto);
            if (actor != null)
            {
                dto = this.mapper.Map<ActorDetailsDto>(actor);

                var movieIds = await context.ActorMovies.Where(x => x.ActorId == id).Select(x => x.MovieId).ToListAsync();
                if (movieIds.Any())
                {
                    var movies = await context.Movies.Where(x => movieIds.Contains(x.Id)).ToListAsync();
                    dto.Movies = this.mapper.Map<IEnumerable<MovieDto>>(movies);
                }
            }

            return dto;
        }

        public async Task<bool> UpdateActorAsync(Guid id, EditActorDto editActorDto)
        {
            var context = this.contextFactory.CreateDbContext();
            var actor = await context.Actors.FirstOrDefaultAsync(x=> x.Id == id);
            if (actor != null)
            {
                this.mapper.Map(editActorDto, actor);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteActorAsync(Guid id)
        {
            var context = this.contextFactory.CreateDbContext();
            var actor = await context.Actors.FindAsync(id);
            if (actor != null)
            {
                context.Remove(actor);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
