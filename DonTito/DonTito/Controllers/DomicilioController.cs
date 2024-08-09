using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomicilioController : ControllerBase
    {
        private readonly IDomicilioService _service;

        public DomicilioController(IDomicilioService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/domicilios")]
        public async Task<IEnumerable<DomicilioDtoOut>> GetDomicilio()
        {
            return await _service.GetDomicilio();
        }


        [HttpGet("api/v1/domicilio/{id}")]
        public async Task<ActionResult<DomicilioDtoOut>> GetDomicilioDtoById(int id)
        {
            var domicilio = await _service.GetDomicilioDtoById(id);
            if (domicilio is null) 
                return NotFound(id);
            return domicilio;
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/domicilio")]
        public async Task<IActionResult> Create(DomicilioDtoIn domicilioDtoIn)
        {
            var newDomicilio = await _service.Create(domicilioDtoIn);

            return CreatedAtAction(nameof(GetDomicilioDtoById), new { id = newDomicilio.Id }, newDomicilio);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DomicilioDtoIn domicilioDtoIn)
        {
            if (id != domicilioDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({domicilioDtoIn.Id}) del cuerpo de la solicitud." });

            var domicilioToUpdate = await _service.GetDomicilioDtoById(id);

            if (domicilioToUpdate is not null)
            {
                await _service.Update(id, domicilioDtoIn);
                return NoContent();

            }
            else
            {
                return NotFound(id);
            }
        }


        //ELIMINAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var toDelete = await _service.GetDomicilioDtoById(id);

            if (toDelete is not null)
            {
                await _service.Delete(id);
                return Ok();
            }
            else
            {
                return NotFound(id);

            }
        }
    }
}
