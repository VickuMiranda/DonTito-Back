﻿using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace DonTito.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("api/v1/producto/buscarmodelo/{model}")]
        public async Task<IEnumerable<ProductoDtoOut>> GetProductoByModelo(string model)
        {
            return await _service.GetProductoByModelo(model);
        }


        //AGREGAR
        [HttpPost("api/v1/agregar/producto")]
        public async Task<IActionResult> Create(ProductoDtoIn productoDtoIn)
        {
            var newProducto = await _service.Create(productoDtoIn);

            return CreatedAtAction(nameof(GetProductoDtoById), new { id = newProducto.Id }, newProducto);
        }


        //EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoDtoIn productoDtoIn)
        {
            if (id != productoDtoIn.Id)
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el ID({productoDtoIn.Id}) del cuerpo de la solicitud." });

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