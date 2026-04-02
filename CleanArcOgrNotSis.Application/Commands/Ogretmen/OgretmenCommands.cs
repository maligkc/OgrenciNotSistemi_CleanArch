using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Domain.Entities;
using MediatR;

namespace CleanArcOgrNotSis.Application.Commands;

public class OgretmenCommands
{
    public record OgretmenEkleCommand(OgretmenDto OgretmenDto) : IRequest<OgretmenDto>;
    
    public record OgretmenGuncelleCommand(OgretmenDto OgretmenDto) : IRequest<OgretmenDto>;

    public record OgretmenSilCommand(int Id) : IRequest<bool>;
}

public class OgretmenEkleCommandHandler : IRequestHandler<OgretmenCommands.OgretmenEkleCommand, OgretmenDto>
{
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IMapper _mapper;

    public OgretmenEkleCommandHandler(IOgretmenRepository ogretmenRepository, IMapper mapper)
    {
        _ogretmenRepository = ogretmenRepository;
        _mapper = mapper;
    }

    public async Task<OgretmenDto> Handle(OgretmenCommands.OgretmenEkleCommand request, CancellationToken cancellationToken)
    {
        var ogretmen = _mapper.Map<Ogretmen>(request.OgretmenDto);
        var eklenenOgretmen = await _ogretmenRepository.EkleAsync(ogretmen);
        return _mapper.Map<OgretmenDto>(eklenenOgretmen);
    }
}

public class OgretmenGuncelleCommandHandler : IRequestHandler<OgretmenCommands.OgretmenGuncelleCommand, OgretmenDto>
{
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IMapper _mapper;

    public OgretmenGuncelleCommandHandler(IOgretmenRepository ogretmenRepository, IMapper mapper)
    {
        _ogretmenRepository = ogretmenRepository;
        _mapper = mapper;
    }

    public async Task<OgretmenDto> Handle(OgretmenCommands.OgretmenGuncelleCommand request, CancellationToken cancellationToken)
    {
        var ogretmen = _mapper.Map<Ogretmen>(request.OgretmenDto);
        await _ogretmenRepository.GuncelleAsync(ogretmen);
        return _mapper.Map<OgretmenDto>(ogretmen);
    }
}

public class OgretmenSilCommandHandler : IRequestHandler<OgretmenCommands.OgretmenSilCommand, bool>
{
    private readonly IOgretmenRepository _ogretmenRepository;

    public OgretmenSilCommandHandler(IOgretmenRepository ogretmenRepository)
    {
        _ogretmenRepository = ogretmenRepository;
    }

    public async Task<bool> Handle(OgretmenCommands.OgretmenSilCommand request, CancellationToken cancellationToken)
    {
        await _ogretmenRepository.SilAsync(request.Id);
        return true;
    }
}