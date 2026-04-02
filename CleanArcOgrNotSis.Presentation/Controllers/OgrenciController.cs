using System.Security.Claims;
using CleanArcOgrNotSis.Application.Commands;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Queries.Ogrenci;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcOgrNotSis.Presentation.Controllers;

[ApiController]
[Route("api/{controller}")]
[Authorize]
public class OgrenciController : ControllerBase
{
    private readonly IMediator _mediator;

    public OgrenciController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Ogretmen")]
    public async Task<ActionResult<IEnumerable<OgrenciDto>>> GetAll()
    {
        var ogrenciler = await _mediator.Send(new OgrenciQueries.TumOgrencileriGetirQuery());
        return Ok(ogrenciler);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Ogretmen,Ogrenci")]
    public async Task<ActionResult<OgrenciDto>> GetById(int id)
    {
        var ogrenci = await _mediator.Send(new OgrenciQueries.IdIleOgrenciGetirQuery(id));
        return Ok(ogrenci);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin öğrenci kaydı ekleyebilir
    public async Task<ActionResult<OgrenciDto>> Register([FromBody] OgrenciDto ogrenciDto)
    {
        var result = await _mediator.Send(new OgrenciCommand.OgrenciEkleCommand(ogrenciDto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Ogretmen")] // Öğrenci kendi kaydını güncelleyebilir
    public async Task<ActionResult<OgrenciDto>> Update(int id, [FromBody] OgrenciDto ogrenciDto)
    {
        if (id != ogrenciDto.Id)
            return BadRequest();
        
        // Öğrenci sadece kendi kaydını güncelleyebilir
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (userRole == "Ogrenci" && id.ToString() != userId)
        {
            return Forbid();
        }

        await _mediator.Send(new OgrenciCommand.OgrenciGuncelleCommand(ogrenciDto));
        return Ok("Öğrenci başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin öğrenci silebilir
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new OgrenciCommand.OgrenciSilCommand(id));
        return Ok("Öğrenci başarıyla silindi");
    }
}