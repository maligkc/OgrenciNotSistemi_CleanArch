using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Application.Mappings;

public class OgrenciMappingProfile : Profile
{
    public OgrenciMappingProfile()
    {
        CreateMap<Ogrenci, OgrenciDto>().ReverseMap();
    }
}