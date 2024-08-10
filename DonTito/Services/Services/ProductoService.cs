using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Request;
using Core.Response;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Interfaces;

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
                    NombreModelo = p.IdModeloNavigation.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Producto?> GetById(int id)
        {
            return await _context.Producto.FindAsync(id);
        }
        public async Task<Producto> Create(ProductoDtoIn newProductoDto)
        {
            var productoDto = await GetProductoByNombre(newProductoDto.Nombre);
            if (productoDto is not null)
            {
                // Aquí necesitas convertir ProductoDtoOut a Producto si es necesario
                var existe = await _context.Producto.FindAsync(productoDto.Id);
                return existe;
            }

            // Si no existe, crea un nuevo producto
            var newProducto = new Producto
            {
                Nombre = newProductoDto.Nombre,
                Precio = newProductoDto.Precio,
                Codigo = newProductoDto.Codigo,
                Descripcion = newProductoDto.Descripcion,
                IdModelo = newProductoDto.IdModelo
            };

            _context.Producto.Add(newProducto);
            await _context.SaveChangesAsync();

            return newProducto;
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
                    NombreModelo = p.IdModeloNavigation.Nombre

                }).ToArrayAsync();
        }

        public async Task<ProductoDtoOut?> GetProductoByNombre(string nombre)
        {

            return await _context.Producto
                .Where(p => p.Nombre == nombre)
                .Select(p => new ProductoDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    NombreModelo = p.IdModeloNavigation.Nombre

                }).SingleOrDefaultAsync();
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
                    NombreModelo = p.IdModeloNavigation.Nombre
                }).ToArrayAsync();
        }

        public async Task Update(int id, ProductoDtoIn productoDtoIn)
        {
            var existingProducto = await GetById(id);
            if (existingProducto is not null)
            {
                existingProducto.Nombre = productoDtoIn.Nombre;
                existingProducto.Precio = productoDtoIn.Precio;
                existingProducto.Codigo = productoDtoIn.Codigo;
                existingProducto.Descripcion = productoDtoIn.Descripcion;
                existingProducto.IdModelo = productoDtoIn.IdModelo;
                await _context.SaveChangesAsync();
            }
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
    }
}
