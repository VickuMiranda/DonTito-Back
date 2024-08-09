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
    public class ProvinciaService : IProvinciaService
    {
        private readonly DonTitoContext _context;
        public ProvinciaService(DonTitoContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<ProvinciaDtoOut>> GetProvincia()
        {
            return await _context.Provincia.Select(p => new ProvinciaDtoOut
            {
                Id = p.Id,
                Nombre = p.Nombre
            }).ToArrayAsync();
        }

        public async Task<ProvinciaDtoOut?> GetProvinciaDtoById(int id)
        {
            return await _context.Provincia
                .Where(p => p.Id == id)
                .Select(p => new ProvinciaDtoOut
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Provincia?> GetById(int id)
        {
            return await _context.Provincia.FindAsync(id);
        }

        public async Task<Provincia> Create(ProvinciaDtoIn newProvinciaDto)
        {
            var newProvincia = new Provincia();

            newProvincia.Nombre = newProvinciaDto.Nombre;
           
            _context.Provincia.Add(newProvincia);
            await _context.SaveChangesAsync();

            return newProvincia;

        }

        public async Task Update(int id, ProvinciaDtoIn provinciaDtoIn)
        {
            var existingProvincia = await GetById(id);
            if (existingProvincia is not null)
            {
                existingProvincia.Nombre = provinciaDtoIn.Nombre;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Provincia.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
