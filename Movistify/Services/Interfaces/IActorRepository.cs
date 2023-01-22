using Movistify.Dtos;

namespace Movistify.Services.Interfaces
{
    public interface IActorRepository
    {
        Task AddActorAsync(EditActorDto newActor);
        Task<bool> DeleteActorAsync(Guid id);
        Task<ActorDetailsDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ActorDto>> SearchActorsAsync(string nameSearchTerm);
        Task<bool> UpdateActorAsync(Guid id, EditActorDto editActorDto);
    }
}