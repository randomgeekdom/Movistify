
using AutoMapper;
using Movistify.Dtos;
using Movistify.Models;

namespace Movistify.MappingProfiles
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorDto, Actor>();
            CreateMap<EditActorDto, Actor>();
            CreateMap<Actor, ActorDto>();
        }
    }
}
