using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movistify.Dtos;
using Movistify.Models;
using Movistify.Services.Interfaces;

namespace Movistify.Services
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovistifyContext movistifyContext;
        private readonly IMapper mapper;

        public ActorRepository(MovistifyContext movistifyContext, IMapper mapper)
        {
            this.movistifyContext = movistifyContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ActorDto>> SearchActorsAsync(string nameSearchTerm)
        {
            var results = await this.movistifyContext.Actors.Where(x => x.Name.ToLower().Contains(nameSearchTerm)).ToListAsync();
            return this.mapper.Map<IEnumerable<ActorDto>>(results);
        }

        public async Task AddActorAsync(EditActorDto newActor)
        {
            var ActorEntity = this.mapper.Map<Actor>(newActor);
            await this.movistifyContext.Actors.AddAsync(ActorEntity);
            await this.movistifyContext.SaveChangesAsync();
        }

        public async Task<ActorDetailsDto?> GetByIdAsync(Guid id)
        {
            var actor = await this.movistifyContext.Actors.FindAsync(id);

            var dto = default(ActorDetailsDto);
            if (actor != null)
            {
                dto = this.mapper.Map<ActorDetailsDto>(actor);

                var movieIds = await this.movistifyContext.ActorMovies.Where(x => x.ActorId == id).Select(x => x.MovieId).ToListAsync();
                if (movieIds.Any())
                {
                    var movies = await this.movistifyContext.Movies.Where(x => movieIds.Contains(x.Id)).ToListAsync();
                    dto.Movies = this.mapper.Map<IEnumerable<MovieDto>>(movies);
                }
            }

            return dto;
        }

        public async Task<bool> UpdateActorAsync(Guid id, EditActorDto editActorDto)
        {
            var actor = await this.movistifyContext.Actors.FindAsync(id);
            if (actor != null)
            {
                this.mapper.Map(editActorDto, actor);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteActorAsync(Guid id)
        {
            var actor = await this.movistifyContext.Actors.FindAsync(id);
            if (actor != null)
            {
                this.movistifyContext.Remove(actor);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
