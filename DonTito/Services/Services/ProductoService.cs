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
            var newProducto = new Producto();

            newProducto.Nombre = newProductoDto.Nombre;
            newProducto.Precio = newProductoDto.Precio;
            newProducto.Codigo = newProductoDto.Codigo;
            newProducto.Descripcion = newProductoDto.Descripcion;
            newProducto.IdModelo = newProductoDto.IdModelo;

            _context.Producto.Add(newProducto);
            await _context.SaveChangesAsync();

            return newProducto;
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
