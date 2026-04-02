using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Application.Mappings;

public class NotMappingProfile : Profile
{
    public NotMappingProfile()
    {
        CreateMap<Not, NotDto>().ReverseMap();
    }
}