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
    public class PedidoService : IPedidoService
    {
        private readonly DonTitoContext _context;
        public PedidoService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PedidoDtoOut>> GetPedido()
        {
            return await _context.Pedido.Select(p => new PedidoDtoOut
            {
                Id = p.Id,
                Numero = p.Numero,
                Total = p.Total,
                FechaCreacion = p.FechaCreacion
            }).ToArrayAsync();
        }

        public async Task<PedidoDtoOut?> GetPedidoDtoById(int id)
        {
            return await _context.Pedido
                .Where(p => p.Id == id)
                .Select(p => new PedidoDtoOut
                {
                    Id = p.Id,
                    Numero = p.Numero,
                    Total = p.Total,
                    FechaCreacion = p.FechaCreacion
                }).SingleOrDefaultAsync();
        }

        public async Task<Pedido?> GetById(int id)
        {
            return await _context.Pedido.FindAsync(id);
        }
        public async Task<Pedido> Create(PedidoDtoIn newPedidoDto)
        {
            DateTime fechaUtc = DateTime.UtcNow;


            var newPedido = new Pedido
            {
                FechaCreacion = fechaUtc,
            };

            // Agregar el nuevo pedido a la base de datos
            _context.Pedido.Add(newPedido);
            await _context.SaveChangesAsync();

            // Obtener los detalles de pedido asociados a este nuevo pedido
            var pedidoDetalles = await _context.PedidoDetalle
                .Where(pd => pd.IdPedido == newPedido.Id)
                .ToListAsync();

            // Calcular el total sumando los subtotales de los detalles del pedido
            newPedido.Total = pedidoDetalles.Sum(pd => pd.SubTotal);

            // Guardar el cambio del total en la base de datos
            await _context.SaveChangesAsync();

            return newPedido;
        }


        public async Task Update(int id, PedidoDtoIn pedidoDtoIn)
        {
            var existingPedido = await GetById(id);
            if (existingPedido is not null)
            {
                existingPedido.Numero = pedidoDtoIn.Numero;
                existingPedido.Total = pedidoDtoIn.Total;
                existingPedido.FechaCreacion = pedidoDtoIn.FechaCreacion;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Pedido.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
