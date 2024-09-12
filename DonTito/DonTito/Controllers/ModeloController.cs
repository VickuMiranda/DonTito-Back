using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloController : ControllerBase
    {
        private readonly IModeloService _service;

        public ModeloController(IModeloService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/modelos")]
        public async Task<IEnumerable<ModeloDtoOut>> GetModelo()
        {
            return await _service.GetModelo();
        }

        [HttpGet("api/v1/modelo/nombre/{name}")]
        public async Task<ActionResult<ModeloDtoOut>> GetModeloByName(string name)
        {
            var modelo = await _service.GetModeloByName(name);
            if (modelo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(modelo);
            }
        }


        [HttpGet("api/v1/modelo/{id}")]
        public async Task<ActionResult<ModeloDtoOut>> GetModeloDtoById(int id)
        {
            var modelo = await _service.GetModeloDtoById(id);
            if (modelo is null)
            {
                return NotFound(id);
            }
            else
            {
                return modelo;
            }
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/modelo")]
        public async Task<IActionResult> Create(ModeloDtoIn modeloDtoIn)
        {
            var newModelo = await _service.Create(modeloDtoIn);

            return CreatedAtAction(nameof(GetModeloDtoById), new { id = newModelo.Id }, newModelo);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ModeloDtoIn modeloDtoIn)
        {
            if (id != modeloDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({modeloDtoIn.Id}) del cuerpo de la solicitud." });

            var modeloToUpdate = await _service.GetModeloDtoById(id);

            if (modeloToUpdate is not null)
            {
                await _service.Update(id, modeloDtoIn);
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
            var toDelete = await _service.GetModeloDtoById(id);

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
