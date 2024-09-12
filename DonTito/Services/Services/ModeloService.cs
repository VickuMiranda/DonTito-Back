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
    public class ModeloService : IModeloService
    {
        private readonly DonTitoContext _context;
        public ModeloService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ModeloDtoOut>> GetModelo()
        {
            return await _context.Modelo.Select(m => new ModeloDtoOut
            {
                Id = m.Id,
                Nombre = m.Nombre,
                NombreMarca = m.IdMarcaNavigation.Nombre
            }).ToArrayAsync();
        }

        public async Task<ModeloDtoOut?> GetModeloDtoById(int id)
        {
            return await _context.Modelo
                .Where(m => m.Id == id)
                .Select(m => new ModeloDtoOut
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    NombreMarca = m.IdMarcaNavigation.Nombre
                }).SingleOrDefaultAsync();
        }
        public async Task<ModeloDtoOut?> GetModeloByName(string name)
        {
            return await _context.Modelo
                .Where(m => m.Nombre == name)
                .Select(m => new ModeloDtoOut
            {
                Nombre = m.Nombre,
                NombreMarca = m.IdMarcaNavigation.Nombre
            }).SingleOrDefaultAsync();
        }
        public async Task<Modelo?> GetById(int id)
        {
            return await _context.Modelo.FindAsync(id);
        }
        public async Task<Modelo> Create(ModeloDtoIn newModeloDto)
        {
            var newModelo = new Modelo();

            newModelo.Nombre = newModeloDto.Nombre;
            newModelo.IdMarca = newModeloDto.IdMarca;

            _context.Modelo.Add(newModelo);
            await _context.SaveChangesAsync();

            return newModelo;

        }


        public async Task Update(int id, ModeloDtoIn modeloDtoIn)
        {
            var existingModelo = await GetById(id);
            if (existingModelo is not null)
            {
                existingModelo.Nombre = modeloDtoIn.Nombre;
                existingModelo.IdMarca = modeloDtoIn.IdMarca;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Modelo.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
