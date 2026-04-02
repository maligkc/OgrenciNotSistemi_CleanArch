using CleanArcOgrNotSis.Application.Commands.Auth;
using CleanArcOgrNotSis.Application.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcOgrNotSis.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("kayit")]
    public async Task<ActionResult<AuthResponseDto>> KayitOl([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _mediator.Send(new AuthCommands.KayitOlCommand(registerDto));

            if (!result.Basarili)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (ValidationException ex)
        {
            var hatalar = ex.Errors
                .Select(e => new { Alan = e.PropertyName, Mesaj = e.ErrorMessage })
                .ToList();
            return BadRequest(new { Basarili = false, Hatalar = hatalar });
        }
    }

    [HttpPost("giris")]
    public async Task<ActionResult<AuthResponseDto>> GirisYap([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _mediator.Send(new AuthCommands.GirisYapCommand(loginDto));
            if (!result.Basarili)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        catch (ValidationException ex)
        {
            var hatalar = ex.Errors
                .Select(e => new { Alan = e.PropertyName, Mesaj = e.ErrorMessage })
                .ToList();

            return BadRequest(new { Basarili = false, Hatalar = hatalar });
        }
    }
    
}