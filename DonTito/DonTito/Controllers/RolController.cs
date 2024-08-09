using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _service;

        public RolController(IRolService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/rol")]
        public async Task<IEnumerable<RolDtoOut>> GetRol()
        {
            return await _service.GetRol();
        }


        [HttpGet("api/v1/rol/{id}")]
        public async Task<ActionResult<RolDtoOut>> GetRolDtoById(int id)
        {
            var rol = await _service.GetRolDtoById(id);
            if (rol is null)
            {
                return NotFound(id);
            }
            else
            {
                return rol;
            }
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/rol")]
        public async Task<IActionResult> Create(RolDtoIn rolDtoIn)
        {
            var newRol = await _service.Create(rolDtoIn);

            return CreatedAtAction(nameof(GetRolDtoById), new { id = newRol.Id }, newRol);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RolDtoIn rolDtoIn)
        {
            if (id != rolDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({rolDtoIn.Id}) del cuerpo de la solicitud." });

            var rolToUpdate = await _service.GetRolDtoById(id);

            if (rolToUpdate is not null)
            {
                await _service.Update(id, rolDtoIn);
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

            var toDelete = await _service.GetRolDtoById(id);

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
