using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Queries.Not;

public class NotQueries
{
    public record TumNotlariGetirQuery : IRequest<IEnumerable<NotDto>>;

    public record IdIleNotGetirQuery(int Id) : IRequest<NotDto?>;

    public record OgrenciNotlariGetirQuery(int OgrenciId) : IRequest<IEnumerable<NotDto>>;

    public record DersNotlariGetirQuery(int DersId) : IRequest<IEnumerable<NotDto>>;
}

public class TumNotlariGetirQueryHandler : IRequestHandler<NotQueries.TumNotlariGetirQuery, IEnumerable<NotDto>>
{
    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;

    public TumNotlariGetirQueryHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NotDto>> Handle(NotQueries.TumNotlariGetirQuery request, CancellationToken cancellationToken)
    {
        var notlar = await _notRepository.TumNotlariGetirAsync(cancellationToken);
        return _mapper.Map<IEnumerable<NotDto>>(notlar);
    }
}

public class IdIleNotGetirQueryHandler : IRequestHandler<NotQueries.IdIleNotGetirQuery, NotDto?>
{
    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;

    public IdIleNotGetirQueryHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    public async Task<NotDto?> Handle(NotQueries.IdIleNotGetirQuery request, CancellationToken cancellationToken)
    {
        var not = await _notRepository.IdIleGetir(request.Id, cancellationToken);
        return _mapper.Map<NotDto>(not);
    }
}

public class OgrenciNotlariGetirQueryHandler : IRequestHandler<NotQueries.OgrenciNotlariGetirQuery, IEnumerable<NotDto>>
{
    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;

    public OgrenciNotlariGetirQueryHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NotDto>> Handle(NotQueries.OgrenciNotlariGetirQuery request, CancellationToken cancellationToken)
    {
        var ogrenciNotlari = await _notRepository.OgrenciNotlariniGetir(request.OgrenciId, cancellationToken);
        return _mapper.Map<IEnumerable<NotDto>>(ogrenciNotlari);
    }
}

public class DersNotlariGetirQueryHandler : IRequestHandler<NotQueries.DersNotlariGetirQuery, IEnumerable<NotDto>>
{
    public DersNotlariGetirQueryHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;


    public async Task<IEnumerable<NotDto>> Handle(NotQueries.DersNotlariGetirQuery request, CancellationToken cancellationToken)
    {
        var dersNotlari = await _notRepository.DersNotlariniGetir(request.DersId, cancellationToken);
        return _mapper.Map<IEnumerable<NotDto>>(dersNotlari);
    }
}