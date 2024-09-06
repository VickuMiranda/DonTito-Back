using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Interfaces;
using System;

namespace Services
{
    public class ImagenService : IImagenService
    {
        private readonly DonTitoContext _context;

        public ImagenService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<ImagenDtoOut?> GetImagenById(int id)
        {
            return await _context.Imagen
                .Where(p => p.Id == id)
                .Select(p => new ImagenDtoOut
                {
                    Id = p.Id,
                    Url = p.Url,
                    NombreProducto = p.IdProductoNavigation.Nombre,


                }).SingleOrDefaultAsync();
        }

        public async Task<int> SubirImagenes(List<IFormFile> files, int idPorducto)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("No se han proporcionado imágenes.");
            }

            var imagen = new Imagen
            {
                IdProducto = idPorducto,
                Url = new List<byte[]>()
            };

            foreach (var file in files)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                imagen.Url.Add(memoryStream.ToArray());
            }

            //MemoryStream: Crea un flujo de memoria(MemoryStream) para leer el contenido del archivo.
            //CopyToAsync: Copia de manera asíncrona los datos del archivo en el flujo de memoria.
            //ToArray: Convierte el contenido del flujo de memoria a un array de bytes.

            _context.Imagen.Add(imagen);
            await _context.SaveChangesAsync();

            return imagen.Id;
        }

    }
}