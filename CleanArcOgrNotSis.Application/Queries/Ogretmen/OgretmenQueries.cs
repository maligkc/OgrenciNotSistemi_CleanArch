using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Queries.Ogretmen;

public class OgretmenQueries
{
    public record TumOgretmenleriGetirQuery() : IRequest<IEnumerable<OgretmenDto>>;

    public record IdIleOgretmenGetirQuery(int Id) : IRequest<OgretmenDto?>;
}


public class TumOgretmenleriGetirQueryHandler : IRequestHandler<OgretmenQueries.TumOgretmenleriGetirQuery, IEnumerable<OgretmenDto>>
{
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IMapper _mapper;

    public TumOgretmenleriGetirQueryHandler(IOgretmenRepository ogretmenRepository, IMapper mapper)
    {
        _ogretmenRepository = ogretmenRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OgretmenDto>> Handle(OgretmenQueries.TumOgretmenleriGetirQuery request, CancellationToken cancellationToken)
    {
        var ogretmenler = await _ogretmenRepository.TumOgretmenleriGetirAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OgretmenDto>>(ogretmenler);
    }
}

public class IdIleOgretmenGetirQueryHandler : IRequestHandler<OgretmenQueries.IdIleOgretmenGetirQuery, OgretmenDto?>
{
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IMapper _mapper;

    public IdIleOgretmenGetirQueryHandler(IOgretmenRepository ogretmenRepository, IMapper mapper)
    {
        _ogretmenRepository = ogretmenRepository;
        _mapper = mapper;
    }

    public async Task<OgretmenDto?> Handle(OgretmenQueries.IdIleOgretmenGetirQuery request, CancellationToken cancellationToken)
    {
        var ogretmen = await _ogretmenRepository.IdIleGetirAsync(request.Id, cancellationToken);
        return ogretmen == null ? null : _mapper.Map<OgretmenDto>(ogretmen);
    }
}