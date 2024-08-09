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
    public class ProvinciaController : ControllerBase
    {
        private readonly IProvinciaService _service;

        public ProvinciaController(IProvinciaService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/provincias")]
        public async Task<IEnumerable<ProvinciaDtoOut>> GetProvincia()
        {
            return await _service.GetProvincia();
        }



        [HttpGet("api/v1/provincia/{id}")]
        public async Task<ActionResult<ProvinciaDtoOut>> GetProvinciaById(int id)
        {
            var provincia = await _service.GetProvinciaDtoById(id);
            if (provincia is null)
            {
                return NotFound(id);
            }
            else
            {
                return provincia;
            }
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/provincia")]
        public async Task<IActionResult> Create(ProvinciaDtoIn provinciaDtoIn)
        {
            var newProvincia = await _service.Create(provinciaDtoIn);

            return CreatedAtAction(nameof(GetProvinciaById), new { id = newProvincia.Id }, newProvincia);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProvinciaDtoIn provinciaDtoIn)
        {
            if (id != provinciaDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({provinciaDtoIn.Id}) del cuerpo de la solicitud." });

            var provinciaToUpdate = await _service.GetProvinciaDtoById(id);

            if (provinciaToUpdate is not null)
            {
                await _service.Update(id, provinciaDtoIn);
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
            var toDelete = await _service.GetProvinciaDtoById(id);

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
