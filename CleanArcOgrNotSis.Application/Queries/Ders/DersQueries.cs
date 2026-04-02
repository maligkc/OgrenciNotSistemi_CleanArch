using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Queries;

public class DersQueries
{
    public record TumDersleriGetirQuery : IRequest<IEnumerable<DersDto>>;

    public record IdIleGetirQuery(int Id) : IRequest<DersDto?>;
}

public class TumDersleriGetirQueryHandler : IRequestHandler<DersQueries.TumDersleriGetirQuery, IEnumerable<DersDto>>
{
    private readonly IDersRepository _dersRepository;
    private readonly IMapper _mapper;

    public TumDersleriGetirQueryHandler(IDersRepository dersRepository, IMapper mapper)
    {
        _dersRepository = dersRepository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<DersDto>> Handle(DersQueries.TumDersleriGetirQuery request, CancellationToken cancellationToken)
    {
        var dersler = await _dersRepository.TumDersleriGetirAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DersDto>>(dersler);
    }
}

public class IdIleGetirQueryHandler : IRequestHandler<DersQueries.IdIleGetirQuery, DersDto?>
{
    private readonly IDersRepository _dersRepository;
    private readonly IMapper _mapper;

    public IdIleGetirQueryHandler(IDersRepository dersRepository, IMapper mapper)
    {
        _dersRepository = dersRepository;
        _mapper = mapper;
    }

    public async Task<DersDto?> Handle(DersQueries.IdIleGetirQuery request, CancellationToken cancellationToken)
    {
        var ders = await _dersRepository.IdIleGetirAsync(request.Id, cancellationToken);

        if (ders == null)
        {
            return null;
        }
        
        return _mapper.Map<DersDto>(ders);
    }
}