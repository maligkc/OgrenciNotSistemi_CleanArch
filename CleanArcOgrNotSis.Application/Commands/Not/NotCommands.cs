using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Commands;

public class NotCommands
{
    public record NotEkleCommand(NotEkleDto Not) : IRequest<NotDto>;
    public record NotGuncelleCommand(NotDto Not) : IRequest<NotDto>;
    public record NotSilCommand(int Id) : IRequest<bool>;
}

public class NotEkleCommandHandler : IRequestHandler<NotCommands.NotEkleCommand, NotDto>
{
    private readonly INotRepository _notRepository;
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IDersRepository _dersRepository;
    private readonly IMapper _mapper;

    public NotEkleCommandHandler
        (INotRepository notRepository, IMapper mapper, IOgrenciRepository ogrenciRepository, IDersRepository dersRepository)
    {
        _notRepository = notRepository;
        _ogrenciRepository = ogrenciRepository;
        _dersRepository = dersRepository;
        _mapper = mapper;
    }


    public async Task<NotDto> Handle(NotCommands.NotEkleCommand request, CancellationToken cancellationToken)
    {
        // öğrenci id ile öğrenciyi çek
        var ogrenci = await _ogrenciRepository.IdIleGetir(request.Not.OgrenciId, cancellationToken);
        if (ogrenci == null)
        {
            throw new Exception($"ID = {request.Not.OgrenciId} olan öğrenci bulunamadı");
        }
        
        // ders id ile dersi çek
        var ders = await _dersRepository.IdIleGetirAsync(request.Not.DersId, cancellationToken);
        if (ders == null)
        {
            throw new Exception($"ID = {request.Not.DersId} olan ders bulunamadı");
        }

        var not = new Not()
        {
            OgrenciId = request.Not.OgrenciId,
            DersId = request.Not.DersId,
            Deger = request.Not.Deger,
            Tarih = DateTime.UtcNow,
            OgrenciAdSoyad = $"{ogrenci.Ad} {ogrenci.Soyad}",
            DersAd = ders.Ad,
            DersKod = ders.Kod
        };

        var eklenenNot = await _notRepository.EkleAsync(not);
        return _mapper.Map<NotDto>(eklenenNot);

    }
}


public class NotGuncelleCommandHandler : IRequestHandler<NotCommands.NotGuncelleCommand, NotDto>
{
    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;

    public NotGuncelleCommandHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    public async Task<NotDto> Handle(NotCommands.NotGuncelleCommand request, CancellationToken cancellationToken)
    {
        var not = _mapper.Map<Not>(request.Not);
        await _notRepository.GuncelleAsync(not);
        return _mapper.Map<NotDto>(not);
    }
}


public class NotSilCommandHandler : IRequestHandler<NotCommands.NotSilCommand, bool>
{
    private readonly INotRepository _notRepository;
    private readonly IMapper _mapper;

    public NotSilCommandHandler(INotRepository notRepository, IMapper mapper)
    {
        _notRepository = notRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(NotCommands.NotSilCommand request, CancellationToken cancellationToken)
    {
        await _notRepository.SilAsync(request.Id);
        return true;
    }
}