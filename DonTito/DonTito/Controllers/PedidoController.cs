using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidoController(IPedidoService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/pedidos")]
        public async Task<IEnumerable<PedidoDtoOut>> GetPedido()
        {
            return await _service.GetPedido();
        }


        [HttpGet("api/v1/pedido/{id}")]
        public async Task<ActionResult<PedidoDtoOut>> GetPedidoDtoById(int id)
        {
            var pedido = await _service.GetPedidoDtoById(id);
            if (pedido is null)
                return NotFound(id);
            return pedido;
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/pedido")]
        public async Task<IActionResult> Create(PedidoDtoIn pedidoDtoIn)
        {
            var newPedido = await _service.Create(pedidoDtoIn);

            return CreatedAtAction(nameof(GetPedidoDtoById), new { id = newPedido.Id }, newPedido);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PedidoDtoIn pedidoDtoIn)
        {
            if (id != pedidoDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({pedidoDtoIn.Id}) del cuerpo de la solicitud." });

            var pedidoToUpdate = await _service.GetPedidoDtoById(id);

            if (pedidoToUpdate is not null)
            {
                await _service.Update(id, pedidoDtoIn);
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

            var toDelete = await _service.GetPedidoDtoById(id);

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
