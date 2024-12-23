﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Request;
using Core.Response;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Interfaces;

namespace Services.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        private readonly DonTitoContext _context;


        public PedidoDetalleService(DonTitoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDetalleDtoOut>> GetPedidoDetalle()
        {
            return await _context.PedidoDetalle.Select(p => new PedidoDetalleDtoOut
            {
                Id = p.Id,
                Cantidad = p.Cantidad,
                SubTotal = p.SubTotal,
                NombreProducto = p.IdProductoNavigation.Nombre,
                NumeroPedido = p.IdPedidoNavigation.Numero,
            }).ToArrayAsync();
        }

        public async Task<PedidoDetalleDtoOut?> GetPedidoDetalleDtoById(int id)
        {
            return await _context.PedidoDetalle
                .Where(p => p.Id == id)
                .Select(p => new PedidoDetalleDtoOut
                {
                    Id = p.Id,
                    Cantidad = p.Cantidad,
                    SubTotal = p.SubTotal,
                    NombreProducto = p.IdProductoNavigation.Nombre,
                    NumeroPedido = p.IdPedidoNavigation.Numero
                }).SingleOrDefaultAsync();
        }


        public async Task<IEnumerable<PedidoDetalleDtoOut?>> GetPedidoDetalleByPedido(int pedido)
        {
            return await _context.PedidoDetalle
                .Where(p => p.IdPedidoNavigation.Numero == pedido)
                .Select(p => new PedidoDetalleDtoOut
            {
                Id = p.Id,
                Cantidad = p.Cantidad,
                SubTotal = p.SubTotal,
                NombreProducto = p.IdProductoNavigation.Nombre,
                NumeroPedido = p.IdPedidoNavigation.Numero
            }).ToListAsync();
        }


        public async Task<PedidoDetalle?> GetById(int id)
        {
            return await _context.PedidoDetalle.FindAsync(id);
        }

        //public async Task<PedidoDetalle> Create(PedidoDetalleDtoIn newPedidoDetalleDto)
        //{
        //    var precioProducto = await ObtenerPrecioProducto(newPedidoDetalleDto.IdProducto);
        //    var newPedidoDetalle = new PedidoDetalle
        //    {
        //        Cantidad = newPedidoDetalleDto.Cantidad,
        //        IdProducto = newPedidoDetalleDto.IdProducto,
        //        IdPedido = newPedidoDetalleDto.IdPedido,
        //        SubTotal = newPedidoDetalleDto.Cantidad * precioProducto
        //    };

        //    _context.PedidoDetalle.Add(newPedidoDetalle);
        //    await _context.SaveChangesAsync();

        //    return newPedidoDetalle;
        //}
        public async Task<PedidoDetalle> Create(PedidoDetalleDtoIn newPedidoDetalleDto)
        {
           // var precioProducto = await ObtenerPrecioProducto(newPedidoDetalleDto.IdProducto);
            var newPedidoDetalle = new PedidoDetalle
            {
                Cantidad = newPedidoDetalleDto.Cantidad,
                IdProducto = newPedidoDetalleDto.IdProducto,
                IdPedido = newPedidoDetalleDto.IdPedido,
                SubTotal = newPedidoDetalleDto.SubTotal
            };

            // Agregar el nuevo detalle de pedido a la base de datos
            _context.PedidoDetalle.Add(newPedidoDetalle);
            await _context.SaveChangesAsync();

            // Actualizar el total del pedido al que pertenece este detalle
            //await ActualizarTotalPedido(newPedidoDetalle.IdPedido);

            return newPedidoDetalle;
        }

        //private async Task ActualizarTotalPedido(int idPedido)
        //{
        //    // Obtener todos los detalles asociados al pedido
        //    var pedidoDetalles = await _context.PedidoDetalle
        //        .Where(pd => pd.IdPedido == idPedido)
        //        .ToListAsync();

        //    // Calcular el nuevo total sumando los subtotales de los detalles
        //    var nuevoTotal = pedidoDetalles.Sum(pd => pd.SubTotal);

        //    // Obtener el pedido y actualizar su total
        //    var pedido = await _context.Pedido.FindAsync(idPedido);
        //    if (pedido != null)
        //    {
        //        pedido.Total = nuevoTotal;
        //        await _context.SaveChangesAsync();
        //    }
        //}


        //private async Task<float> ObtenerPrecioProducto(int idProducto)
        //{
        //    var producto = await _context.Producto.FindAsync(idProducto);
        //    return producto?.Precio ?? 0;
        //}

        public async Task Update(int id, PedidoDetalleDtoIn pedidoDetalleDtoIn)
        {
            var existingPedidoDetalle = await GetById(id);
            if (existingPedidoDetalle is not null)
            {
                //var precioProducto = await ObtenerPrecioProducto(pedidoDetalleDtoIn.IdProducto);

                existingPedidoDetalle.Cantidad = pedidoDetalleDtoIn.Cantidad;
                existingPedidoDetalle.IdProducto = pedidoDetalleDtoIn.IdProducto;
                existingPedidoDetalle.IdPedido = pedidoDetalleDtoIn.IdPedido;

                // Recalcular el subtotal
                existingPedidoDetalle.SubTotal = pedidoDetalleDtoIn.SubTotal;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.PedidoDetalle.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}

