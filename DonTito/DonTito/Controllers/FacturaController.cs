using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _service;

        public FacturaController(IFacturaService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/facturas")]
        public async Task<IEnumerable<FacturaDtoOut>> GetFactura()
        {
            return await _service.GetFactura();
        }


        [HttpGet("api/v1/factura/{id}")]
        public async Task<ActionResult<FacturaDtoOut>> GetFacturaDtoById(int id)
        {
            var factura = await _service.GetFacturaDtoById(id);
            if (factura is null)
                return NotFound(id);
            return factura;
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/factura")]
        public async Task<IActionResult> Create(FacturaDtoIn facturaDtoIn)
        {
            var newFactura = await _service.Create(facturaDtoIn);

            return CreatedAtAction(nameof(GetFacturaDtoById), new { id = newFactura.Id }, newFactura);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FacturaDtoIn facturaDtoIn)
        {
            if (id != facturaDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({facturaDtoIn.Id}) del cuerpo de la solicitud." });

            var facturaToUpdate = await _service.GetFacturaDtoById(id);

            if (facturaToUpdate is not null)
            {
                await _service.Update(id, facturaDtoIn);
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

            var toDelete = await _service.GetFacturaDtoById(id);

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
