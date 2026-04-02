using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Queries.Ogrenci;

public class OgrenciQueries
{
    public record TumOgrencileriGetirQuery : IRequest<IEnumerable<OgrenciDto>>;

    public record IdIleOgrenciGetirQuery(int Id) : IRequest<OgrenciDto>;
}

public class TumOgrencileriGetirQueryHandler : IRequestHandler<OgrenciQueries.TumOgrencileriGetirQuery, IEnumerable<OgrenciDto>>
{
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IMapper _mapper;

    public TumOgrencileriGetirQueryHandler(IOgrenciRepository ogrenciRepository, IMapper mapper)
    {
        _ogrenciRepository = ogrenciRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OgrenciDto>> Handle(OgrenciQueries.TumOgrencileriGetirQuery request, CancellationToken cancellationToken)
    {
        var ogrenciler = await _ogrenciRepository.TumOgrencileriGetir(cancellationToken);
        return _mapper.Map<IEnumerable<OgrenciDto>>(ogrenciler);
    }
}

public class IdIleOgrenciGetirQueryHandler : IRequestHandler<OgrenciQueries.IdIleOgrenciGetirQuery, OgrenciDto>
{
    private readonly IOgrenciRepository _ogrenciRepository;

    public IdIleOgrenciGetirQueryHandler(IOgrenciRepository ogrenciRepository, IMapper mapper)
    {
        _ogrenciRepository = ogrenciRepository;
        _mapper = mapper;
    }

    private readonly IMapper _mapper;

    public async Task<OgrenciDto> Handle(OgrenciQueries.IdIleOgrenciGetirQuery request, CancellationToken cancellationToken)
    {
        var ogrenci = await _ogrenciRepository.IdIleGetir(request.Id, cancellationToken);
        return _mapper.Map<OgrenciDto>(ogrenci);
    }
}