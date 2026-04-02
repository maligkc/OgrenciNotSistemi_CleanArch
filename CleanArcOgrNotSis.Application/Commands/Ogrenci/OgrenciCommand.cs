using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Commands;

public class OgrenciCommand
{
    public record OgrenciEkleCommand(OgrenciDto Ogrenci) : IRequest<OgrenciDto>;
    public record OgrenciGuncelleCommand(OgrenciDto Ogrenci) : IRequest<OgrenciDto>;
    public record OgrenciSilCommand(int Id) : IRequest<bool>;
}

public class OgrenciEkleCommandHandler : IRequestHandler<OgrenciCommand.OgrenciEkleCommand, OgrenciDto>
{
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IMapper _mapper;

    public OgrenciEkleCommandHandler(IOgrenciRepository ogrenciRepository, IMapper mapper)
    {
        _ogrenciRepository = ogrenciRepository;
        _mapper = mapper;
    }


    public async Task<OgrenciDto> Handle(OgrenciCommand.OgrenciEkleCommand request, CancellationToken cancellationToken)
    {
        var ogr = _mapper.Map<Ogrenci>(request.Ogrenci);
        ogr.KayitTarihi = DateTime.UtcNow;
        var eklenen = await _ogrenciRepository.EkleAsync(ogr);

        return _mapper.Map<OgrenciDto>(eklenen);
    }
}


public class OgrenciGuncelleCommandHandler : IRequestHandler<OgrenciCommand.OgrenciGuncelleCommand, OgrenciDto>
{
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IMapper _mapper;

    public OgrenciGuncelleCommandHandler(IOgrenciRepository ogrenciRepository, IMapper mapper)
    {
        _ogrenciRepository = ogrenciRepository;
        _mapper = mapper;
    }

    public async Task<OgrenciDto> Handle(OgrenciCommand.OgrenciGuncelleCommand request, CancellationToken cancellationToken)
    {
        var ogrenci = _mapper.Map<Ogrenci>(request.Ogrenci);
        await _ogrenciRepository.GuncelleAsync(ogrenci);
        return _mapper.Map<OgrenciDto>(ogrenci);
    }
}


public class OgrenciSilCommandHandler : IRequestHandler<OgrenciCommand.OgrenciSilCommand, bool>
{
    private readonly IOgrenciRepository _ogrenciRepository;

    public OgrenciSilCommandHandler(IOgrenciRepository ogrenciRepository)
    {
        _ogrenciRepository = ogrenciRepository;
    }

    public async Task<bool> Handle(OgrenciCommand.OgrenciSilCommand request, CancellationToken cancellationToken)
    {
        await _ogrenciRepository.SilAsync(request.Id);
        return true;
    }
}