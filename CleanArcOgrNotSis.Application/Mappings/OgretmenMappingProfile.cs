using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Application.Mappings;

public class OgretmenMappingProfile : Profile
{
    public OgretmenMappingProfile()
    {
        CreateMap<Ogretmen, OgretmenDto>().ReverseMap();
    }
}