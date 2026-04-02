using CleanArcOgrNotSis.Application.Commands;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArcOgrNotSis.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DerslerController : ControllerBase
{
       private readonly IMediator _mediator;

       public DerslerController(IMediator mediator)
       {
              _mediator = mediator;
       }

       [HttpGet]
       [Authorize(Roles = "Admin,Ogretmen,Ogrenci")]
       public async Task<ActionResult<IEnumerable<DersDto>>> GetAll()
       {
              var dersler = await _mediator.Send(new DersQueries.TumDersleriGetirQuery());
              
              return Ok(dersler);
       }

       [HttpGet("{id}")]
       [Authorize(Roles = "Admin,Ogretmen,Ogrenci")]
       public async Task<ActionResult<DersDto>> GetById(int id)
       {
              var ders = await _mediator.Send(new DersQueries.IdIleGetirQuery(id));
              if (ders == null)
                     return NotFound();
              return Ok(ders);
       }

       [HttpPost]
       [Authorize(Roles = "Admin,Ogretmen")] // Sadece öğretmen ve admin ders ekleyebilir
       public async Task<ActionResult<DersDto>> Create([FromBody] DersDto dersDto)
       {
              var result = await _mediator.Send(new DersCommands.DersEkleCommand(dersDto));
              return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
       }

       [HttpPut("{id}")]
       [Authorize(Roles = "Admin,Ogretmen")]
       public async Task<ActionResult<DersDto>> Update(int id, [FromBody] DersDto dersDto)
       {
              if (id != dersDto.Id)
              {
                     return BadRequest();
              }

              var result = await _mediator.Send(new DersCommands.DersGuncelleCommand(dersDto));
              return Ok($"{result.Ad} dersi başarıyla güncellendi");
       }


       [HttpDelete("{id}")]
       [Authorize(Roles = "Admin,Ogretmen")]
       public async Task<ActionResult<DersDto>> Delete(int id)
       {
             var ders = await _mediator.Send(new DersQueries.IdIleGetirQuery(id));

             if (ders == null)
             {
                    return NotFound();
             }

             await _mediator.Send(new DersCommands.DersSilCommand(id));
             return Ok("Ders Kaydı Başarıyla Silindi");
       }
}