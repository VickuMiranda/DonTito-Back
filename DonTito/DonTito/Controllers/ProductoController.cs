using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet("api/v1/productos")]
        public async Task<IEnumerable<ProductoDtoOut>> GetProducto()
        {
            return await _service.GetProducto();
        }


        [HttpGet("api/v1/producto/{id}")]
        public async Task<ActionResult<ProductoDtoOut>> GetProductoDtoById(int id)
        {
            var producto = await _service.GetProductoDtoById(id);
            if (producto is null)
                return NotFound(id);
            return producto;
        }

        //Buscar Por modelo
        [HttpGet("api/v1/producto/buscarModelo/{modelo}")]
        public async Task<IEnumerable<ProductoDtoOut>> GetProductoByModelo(string modelo)
        {
            return await _service.GetProductoByModelo(modelo);
        }

        //Buscar por Marca
        [HttpGet("api/v1/producto/buscarMarca/{marca}")]
        public async Task<IEnumerable<ProductoDtoOut>> GetProductoByMarca(string marca)
        {
            return await _service.GetProductoByMarca(marca);
        }

        //Buscar por nombre de Producto
        [HttpGet("api/v1/producto/buscarNombre/{nombre}")]
        public async Task<IEnumerable<ProductoDtoOut?>> GetProductoByNombre(string nombre)
        {
            return await _service.GetProductoByNombre(nombre);
        }



        //AGREGAR
         [HttpPost("api/v1/agregar/producto")]
        [RequestSizeLimit(1_000_000)]
        public async Task<IActionResult> Create([FromForm] ProductoDtoIn productoDtoIn, IFormFile files)
         {        
           var newProducto = await _service.Create(productoDtoIn, files);
         return CreatedAtAction(nameof(GetProductoDtoById), new { id = newProducto.Id }, newProducto);
         }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoDtoIn productoDtoIn)
        {
            //if (id != productoDtoIn.Id)
            //    return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({productoDtoIn.Id}) del cuerpo de la solicitud." });

            var productoToUpdate = await _service.GetProductoDtoById(id);

            if (productoToUpdate is not null)
            {
                await _service.Update(id, productoDtoIn);
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
            var toDelete = await _service.GetProductoDtoById(id);

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
