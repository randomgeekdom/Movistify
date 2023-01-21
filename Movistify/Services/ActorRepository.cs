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

        public async Task<ActorDto> GetByIdAsync(Guid id)
        {
            var Actor = await this.movistifyContext.Actors.FindAsync(id);
            return this.mapper.Map<ActorDto>(Actor);
        }

        public async Task<bool> UpdateActorAsync(Guid id, EditActorDto editActorDto)
        {
            var Actor = await this.movistifyContext.Actors.FindAsync(id);
            if (Actor != null)
            {
                this.mapper.Map(editActorDto, Actor);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteActorAsync(Guid id)
        {
            var Actor = await this.movistifyContext.Actors.FindAsync(id);
            if (Actor != null)
            {
                this.movistifyContext.Remove(Actor);
                await this.movistifyContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
