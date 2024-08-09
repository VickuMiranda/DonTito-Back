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
    public class RolService : IRolService
    {
        private readonly DonTitoContext _context;
        public RolService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RolDtoOut>> GetRol()
        {
            return await _context.Rol.Select(r => new RolDtoOut
            {
                Id = r.Id,
                Nombre = r.Nombre
            }).ToArrayAsync();
        }

        public async Task<RolDtoOut?> GetRolDtoById(int id)
        {
            return await _context.Rol
                .Where(r => r.Id == id)
                .Select(r => new RolDtoOut
                {
                    Id = r.Id,
                    Nombre = r.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Rol?> GetById(int id)
        {
            return await _context.Rol.FindAsync(id);
        }
        public async Task<Rol> Create(RolDtoIn newRolDto)
        {
            var newRol = new Rol();

            newRol.Nombre = newRolDto.Nombre;

            _context.Rol.Add(newRol);
            await _context.SaveChangesAsync();

            return newRol;

        }


        public async Task Update(int id, RolDtoIn rolDtoIn)
        {
            var existingRol = await GetById(id);
            if (existingRol is not null)
            {
                existingRol.Nombre = rolDtoIn.Nombre;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Rol.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
