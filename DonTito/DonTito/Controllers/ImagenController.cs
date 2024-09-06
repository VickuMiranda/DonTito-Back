using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;

namespace PruebaImagen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private readonly IImagenService _service;
       

        public ImagenController(IImagenService service)
        {
            _service = service;
            
        }
        [HttpGet("api/v1/imagen/{id}")]
        public async Task<ActionResult<ImagenDtoOut>> GetImagenById(int id)
        {
            var imagen = await _service.GetImagenById(id);
            if (imagen is null)
            {
                return NotFound(id);
            }
            return imagen;
        }

        [HttpPost]
        [Route("subir")]
        public async Task<IActionResult> SubirImagenes(List<IFormFile> files, int idProducto)
        {
            try
            {
                var imagenId = await _service.SubirImagenes(files, idProducto);
                return Ok(new { Id = imagenId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}