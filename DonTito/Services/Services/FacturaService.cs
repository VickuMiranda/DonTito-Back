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
    public class FacturaService : IFacturaService
    {
        private readonly DonTitoContext _context;
        public FacturaService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FacturaDtoOut>> GetFactura()
        {
            return await _context.Factura.Select(f => new FacturaDtoOut
            {
                Id = f.Id,
                MontoTotal = f.MontoTotal,
                Fecha = f.Fecha,
                Numero = f.Numero,
            }).ToArrayAsync();
        }

        public async Task<FacturaDtoOut?> GetFacturaDtoById(int id)
        {
            return await _context.Factura
                .Where(f => f.Id == id)
                .Select(f => new FacturaDtoOut
                {
                    Id = f.Id,
                    MontoTotal = f.MontoTotal,
                    Fecha = f.Fecha,
                    Numero = f.Numero,
                }).SingleOrDefaultAsync();
        }

        public async Task<Factura?> GetById(int id)
        {
            return await _context.Factura.FindAsync(id);
        }
        public async Task<Factura> Create(FacturaDtoIn newFacturaDto)
        {
            var newFactura = new Factura();

            newFactura.MontoTotal = newFacturaDto.MontoTotal;
            newFactura.Fecha = newFacturaDto.Fecha;
            
            _context.Factura.Add(newFactura);
            await _context.SaveChangesAsync();

            return newFactura;

        }


        public async Task Update(int id, FacturaDtoIn facturaDtoIn)
        {
            var existingFactura = await GetById(id);
            if (existingFactura is not null)
            {
                existingFactura.MontoTotal = facturaDtoIn.MontoTotal;
                existingFactura.Fecha = DateTime.Now;
                existingFactura.Numero = facturaDtoIn.Numero;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Factura.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }

    }
}
