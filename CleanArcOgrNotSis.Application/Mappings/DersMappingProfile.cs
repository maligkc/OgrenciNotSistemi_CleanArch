using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Application.Mappings;

public class DersMappingProfile : Profile
{
    public DersMappingProfile()
    {
        CreateMap<Ders, DersDto>().ReverseMap();
    }
}