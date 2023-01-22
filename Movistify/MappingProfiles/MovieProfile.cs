
using AutoMapper;
using Movistify.Dtos;
using Movistify.Models;
using System.Runtime.CompilerServices;

namespace Movistify.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<MovieDto, Movie>();
            CreateMap<EditMovieDto, Movie>();
            CreateMap<Movie, MovieDto>();
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieRatingDto, MovieRating>();
            CreateMap<MovieRating, MovieRatingDto>();
        }
    }
}
