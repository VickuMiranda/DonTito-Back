using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Services.Services
{
    public class ProductoService : IProductoService
    {
        private readonly DonTitoContext _context;
        public ProductoService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductoDtoOut>> GetProducto()
        {
            return await _context.Producto.Select(p => new ProductoDtoOut
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Codigo = p.Codigo,
                Descripcion = p.Descripcion,
                Imagen = p.Imagen,
                NombreModelo = p.IdModeloNavigation.Nombre
            }).ToArrayAsync();
        }

        public async Task<ProductoDtoOut?> GetProductoDtoById(int id)
        {
            return await _context.Producto
                .Where(p => p.Id == id)
                .Select(p => new ProductoDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen,
                    NombreModelo = p.IdModeloNavigation.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Producto?> GetById(int id)
        {
            return await _context.Producto.FindAsync(id);
        }
        public async Task<Producto> Create(ProductoDtoIn newProductoDto, IFormFile files)
        {
            if (files == null || files.Length == 0)
            {
                throw new ArgumentException("No se han proporcionado imágenes.");
            }

            // Convierte el archivo de imagen a un array de bytes
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await files.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Crea un nuevo producto
            var newProducto = new Producto
            {
                Nombre = newProductoDto.Nombre,
                Precio = newProductoDto.Precio,
                Codigo = newProductoDto.Codigo,
                Descripcion = newProductoDto.Descripcion,
                Imagen = imageBytes,  // Asigna el array de bytes de la imagen
                IdModelo = newProductoDto.IdModelo,
            };

            // Agrega el nuevo producto a la base de datos y guarda los cambios
            _context.Producto.Add(newProducto);
            await _context.SaveChangesAsync();

            return newProducto;
        }


        public async Task Update(int id, ProductoDtoIn productoDtoIn, IFormFile files)
        {
            var existingProducto = await GetById(id);
            if (existingProducto is not null)
            {
            if (files == null || files.Length == 0)
            {
                throw new ArgumentException("No se han proporcionado imágenes.");
            }
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await files.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                existingProducto.Nombre = productoDtoIn.Nombre;
                existingProducto.Precio = productoDtoIn.Precio;
                existingProducto.Codigo = productoDtoIn.Codigo;
                existingProducto.Descripcion = productoDtoIn.Descripcion;
                existingProducto.Imagen = imageBytes;
                existingProducto.IdModelo = productoDtoIn.IdModelo;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductoDtoOut>> GetProductoByModelo(string modelo)
        {

            return await _context.Producto
                .Where(p => p.IdModeloNavigation.Nombre == modelo)
                .Select(p => new ProductoDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen,
                    NombreModelo = p.IdModeloNavigation.Nombre

                }).ToArrayAsync();
        }

        public async Task<IEnumerable<ProductoDtoOut?>> GetProductoByNombre(string nombre)
        {

            return await _context.Producto
                .Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()))
                .Select(p => new ProductoDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen,
                    NombreModelo = p.IdModeloNavigation.Nombre

                }).ToListAsync();
        }

        public async Task<IEnumerable<ProductoDtoOut>> GetProductoByMarca(string marca)
        {
            return await _context.Producto
                .Where(p => p.IdModeloNavigation.IdMarcaNavigation.Nombre.Equals(marca))
                .Select(p => new ProductoDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    Imagen = p.Imagen,
                    NombreModelo = p.IdModeloNavigation.Nombre
                }).ToArrayAsync();
        }

     

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Producto.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }


        // Método para convertir el array de bytes a base64
        public string ConvertToBase64(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;  // O una cadena vacía, dependiendo de tu caso de uso
            }

            return Convert.ToBase64String(imageBytes);
        }

    }
}
