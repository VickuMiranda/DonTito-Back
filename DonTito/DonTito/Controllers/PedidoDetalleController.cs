using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoDetalleController : ControllerBase
    {
        private readonly IPedidoDetalleService _service;

        public PedidoDetalleController(IPedidoDetalleService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/pedidoDetalle")]
        public async Task<IEnumerable<PedidoDetalleDtoOut>> GetPedidoDetalle()
        {
            return await _service.GetPedidoDetalle();
        }



        [HttpGet("api/v1/pedidoDetalle/{id}")]
        public async Task<ActionResult<PedidoDetalleDtoOut>> GetPedidoDetalleDtoById(int id)
        {
            var pedidoDetalle = await _service.GetPedidoDetalleDtoById(id);
            if (pedidoDetalle is null)
            {
                return NotFound(id);
            }
            else
            {
                return pedidoDetalle;
            }
        }

        //Buscar Por modelo
        [HttpGet("api/v1/pedidoDetalle/buscarPedido/{pedido}")]
        public async Task<IEnumerable<PedidoDetalleDtoOut?>> GetPedidoDetalleByPedido(int pedido)
        {
            return await _service.GetPedidoDetalleByPedido(pedido);
        }

        //AGREGAR
        [HttpPost("api/v1/agregar/pedidoDetalle")]
        public async Task<IActionResult> Create(PedidoDetalleDtoIn pedidoDetalleDtoIn)
        {
            var newPedidoDetalle = await _service.Create(pedidoDetalleDtoIn);

            return CreatedAtAction(nameof(GetPedidoDetalleDtoById), new { id = newPedidoDetalle.Id }, newPedidoDetalle);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PedidoDetalleDtoIn pedidoDetalleDtoIn)
        {
            if (id != pedidoDetalleDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({pedidoDetalleDtoIn.Id}) del cuerpo de la solicitud." });

            var pedidoDetalleToUpdate = await _service.GetPedidoDetalleDtoById(id);

            if (pedidoDetalleToUpdate is not null)
            {
                await _service.Update(id, pedidoDetalleDtoIn);
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

            var toDelete = await _service.GetPedidoDetalleDtoById(id);

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
