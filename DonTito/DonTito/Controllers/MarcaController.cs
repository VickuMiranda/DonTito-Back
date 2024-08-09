using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService _service;

        public MarcaController(IMarcaService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/marcas")]
        public async Task<IEnumerable<MarcaDtoOut>> GetMarca()
        {
            return await _service.GetMarca();
        }



        [HttpGet("api/v1/marca/{id}")]
        public async Task<ActionResult<MarcaDtoOut>> GetMarcaDtoById(int id)
        {
            var marca = await _service.GetMarcaDtoById(id);
            if (marca is null)
            {
                return NotFound(id);
            }
            else
            {
                return marca;
            }
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/marca")]
        public async Task<IActionResult> Create(MarcaDtoIn marcaDtoIn)
        {
            var newMarca = await _service.Create(marcaDtoIn);

            return CreatedAtAction(nameof(GetMarcaDtoById), new { id = newMarca.Id }, newMarca);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MarcaDtoIn marcaDtoIn)
        {
            if (id != marcaDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({marcaDtoIn.Id}) del cuerpo de la solicitud." });

            var marcaToUpdate = await _service.GetMarcaDtoById(id);

            if (marcaToUpdate is not null)
            {
                await _service.Update(id, marcaDtoIn);
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
            var toDelete = await _service.GetMarcaDtoById(id);

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
