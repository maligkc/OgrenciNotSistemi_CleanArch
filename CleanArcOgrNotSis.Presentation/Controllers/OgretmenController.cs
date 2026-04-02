using System.Security.Claims;
using CleanArcOgrNotSis.Application.Commands;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Queries.Ogretmen;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcOgrNotSis.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OgretmenController : ControllerBase
{
    private readonly IMediator _mediator;

    public OgretmenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")] // Sadece admin tüm öğretmenleri listeleyebilir
    public async Task<ActionResult<IEnumerable<OgretmenDto>>> GetAll()
    {
        var ogretmenler = await _mediator.Send(new OgretmenQueries.TumOgretmenleriGetirQuery());
        return Ok(ogretmenler);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Ogretmen")] // Öğretmen kendi kaydını görebilir
    public async Task<ActionResult<OgretmenDto>> GetById(int id)
    {
        // ogretmen sadece kendi profilini görebilir
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (userRole == "Ogretmen" && id.ToString() != userId)
        {
            return Forbid();
        }

        var ogretmen = await _mediator.Send(new OgretmenQueries.IdIleOgretmenGetirQuery(id));
        
        if (ogretmen == null) return NotFound();

        return Ok(ogretmen);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece admin öğretmen ekleyebilir
    public async Task<ActionResult<OgretmenDto>> Create([FromBody] OgretmenDto ogretmenDto)
    {
        var result = await _mediator.Send(new OgretmenCommands.OgretmenEkleCommand(ogretmenDto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Ogretmen")] // Öğretmen kendi kaydını güncelleyebilir
    public async Task<ActionResult> Update(int id, [FromBody] OgretmenDto ogretmenDto)
    {
        if (id != ogretmenDto.Id)
            return BadRequest();

        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (userRole == "Ogretmen" && id.ToString() != userId)
        {
            return Forbid();
        }

        await _mediator.Send(new OgretmenCommands.OgretmenGuncelleCommand(ogretmenDto));
        return Ok("Öğretmen başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece admin öğretmen silebilir
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new OgretmenCommands.OgretmenSilCommand(id));
        return Ok("Öğretmen başarıyla silindi");
    }
    
}