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
    public class MarcaService : IMarcaService
    {
        private readonly DonTitoContext _context;
        public MarcaService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MarcaDtoOut>> GetMarca()
        {
            return await _context.Marca.Select(m => new MarcaDtoOut
            {
                Id = m.Id,
                Nombre = m.Nombre
            }).ToArrayAsync();
        }

        public async Task<MarcaDtoOut?> GetMarcaDtoById(int id)
        {
            return await _context.Marca
                .Where(m => m.Id == id)
                .Select(m => new MarcaDtoOut
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Marca?> GetById(int id)
        {
            return await _context.Marca.FindAsync(id);
        }
        public async Task<Marca> Create(MarcaDtoIn newMarcaDto)
        {
            var newMarca = new Marca();
            if (string.IsNullOrWhiteSpace(newMarcaDto.Nombre))
            {
                throw new ArgumentException("El nombre de la marca no puede estar vacío.");
            }

            newMarca.Nombre = newMarcaDto.Nombre;

            _context.Marca.Add(newMarca);
            await _context.SaveChangesAsync();

            return newMarca;
        }


        public async Task Update(int id, MarcaDtoIn marcaDtoIn)
        {
            var existingMarca = await GetById(id);
            if (existingMarca is not null)
            {
                existingMarca.Nombre = marcaDtoIn.Nombre;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Marca.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }

    }
}
