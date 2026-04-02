using AutoMapper;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Commands;

public static class DersCommands
{
    public record DersEkleCommand(DersDto ders) : IRequest<DersDto>;
    public record DersGuncelleCommand(DersDto ders) : IRequest<DersDto>;
    public record DersSilCommand(int id) : IRequest<bool>;
}

public class DersEkleCommandHandler : IRequestHandler<DersCommands.DersEkleCommand, DersDto>
{
    private readonly IDersRepository _dersRepository;
    private readonly IMapper _mapper;


    public DersEkleCommandHandler(IDersRepository dersRepository, IMapper mapper)
    {
        _dersRepository = dersRepository;
        _mapper = mapper;
    }

    public async Task<DersDto> Handle(DersCommands.DersEkleCommand request, CancellationToken cancellationToken)
    {
        var ders = _mapper.Map<Ders>(request.ders);
        var eklenen = await _dersRepository.EkleAsync(ders);
        return _mapper.Map<DersDto>(eklenen);
    }
}


public class DersGuncelleCommandHandler : IRequestHandler<DersCommands.DersGuncelleCommand, DersDto>
{
    private readonly IDersRepository _dersRepository;
    private readonly IMapper _mapper;


    public DersGuncelleCommandHandler(IDersRepository dersRepository, IMapper mapper)
    {
        _dersRepository = dersRepository;
        _mapper = mapper;
    }

    public async Task<DersDto> Handle(DersCommands.DersGuncelleCommand request, CancellationToken cancellationToken)
    {
        var ders = _mapper.Map<Ders>(request.ders);
        await _dersRepository.GuncelleAsync(ders);
        return _mapper.Map<DersDto>(ders);
    }
}

public class DersSilCommandHandler : IRequestHandler<DersCommands.DersSilCommand, bool>
{
    private readonly IDersRepository _dersRepository;


    public DersSilCommandHandler(IDersRepository dersRepository)
    {
        _dersRepository = dersRepository;
    }

    public async Task<bool> Handle(DersCommands.DersSilCommand request, CancellationToken cancellationToken)
    {
        await _dersRepository.SilAsync(request.id);
        return true;
    }
}