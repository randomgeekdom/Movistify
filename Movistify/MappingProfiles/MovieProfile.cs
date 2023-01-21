
using AutoMapper;
using Movistify.Dtos;
using Movistify.Models;

namespace Movistify.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<MovieDto, Movie>();
            CreateMap<EditMovieDto, Movie>();
            CreateMap<Movie, MovieDto>();
        }
    }
}
