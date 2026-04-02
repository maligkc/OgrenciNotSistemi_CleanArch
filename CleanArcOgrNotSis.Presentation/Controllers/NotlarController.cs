using CleanArcOgrNotSis.Application.Commands;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Queries;
using CleanArcOgrNotSis.Application.Queries.Not;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcOgrNotSis.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotlarController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public NotlarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Ogretmen")] // Sadece öğretmen ve admin tüm notları görebilir
    public async Task<ActionResult<IEnumerable<NotDto>>> GetAll()
    {
        var notlar = await _mediator.Send(new NotQueries.TumNotlariGetirQuery());
        return Ok(notlar);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Ogretmen,Ogrenci")]
    public async Task<ActionResult<NotDto>> GetById(int id)
    {
        var not = await _mediator.Send(new NotQueries.IdIleNotGetirQuery(id));
        if (not == null)
        {
            return NotFound();
        }
        return Ok(not);
    }

    [HttpGet("ogrenci/{ogrenciId}")]
    [Authorize(Roles = "Admin,Ogretmen,Ogrenci")]
    public async Task<ActionResult<IEnumerable<NotDto>>> GetByOgrenciId(int ogrenciId)
    {
        var notlar = await _mediator.Send(new NotQueries.OgrenciNotlariGetirQuery(ogrenciId));
        return Ok(notlar);
    }

    [HttpGet("ders/{dersId}")]
    [Authorize(Roles = "Admin,Ogretmen")]
    public async Task<ActionResult<IEnumerable<NotDto>>> GetByDersId(int dersId)
    {
        var notlar = await _mediator.Send(new NotQueries.DersNotlariGetirQuery(dersId));
        return Ok(notlar);
    }
    
    
    [HttpPost]
    [Authorize(Roles = "Admin,Ogretmen")]
    public async Task<ActionResult<NotDto>> Create([FromBody] NotEkleDto notEkleDto)
    {
        var result = await _mediator.Send(new NotCommands.NotEkleCommand(notEkleDto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        
    }
    
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Ogretmen")]
    public async Task<ActionResult<NotDto>> Update(int id, [FromBody] NotDto notDto)
    {
        if (id != notDto.Id)
        {
            return BadRequest();
        }
        var result = await _mediator.Send(new NotCommands.NotGuncelleCommand(notDto));
        return Ok("Not başarıyla güncellendi");

    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Ogretmen")]
    public async Task<ActionResult<NotDto>> Delete(int id)
    {
        var not = await _mediator.Send(new NotQueries.IdIleNotGetirQuery(id));

        if (not == null)
        {
            return NotFound();
        }
        

        await _mediator.Send(new NotCommands.NotSilCommand(id));
        return Ok("Not başarıyla silindi");
        
    }
    
}