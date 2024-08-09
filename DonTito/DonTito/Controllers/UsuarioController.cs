using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/usuarios")]
        public async Task<IEnumerable<UsuarioDtoOut>> GetUsuario()
        {
            return await _service.GetUsuario();
        }


        [HttpGet("api/v1/usuario/{id}")]
        public async Task<ActionResult<UsuarioDtoOut>> GetUsuarioDtoById(int id)
        {
            var usuario = await _service.GetUsuarioDtoById(id);
            if (usuario is null)
                return NotFound(id);
            return usuario;
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/usuario")]
        public async Task<IActionResult> Create(UsuarioDtoIn usuarioDtoIn)
        {
            var newUsuario = await _service.Create(usuarioDtoIn);

            return CreatedAtAction(nameof(GetUsuarioDtoById), new { id = newUsuario.Id }, newUsuario);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsuarioDtoIn usuarioDtoIn)
        {
            if (id != usuarioDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({usuarioDtoIn.Id}) del cuerpo de la solicitud." });

            var usuarioToUpdate = await _service.GetUsuarioDtoById(id);

            if (usuarioToUpdate is not null)
            {
                await _service.Update(id, usuarioDtoIn);
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

            var toDelete = await _service.GetUsuarioDtoById(id);

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
