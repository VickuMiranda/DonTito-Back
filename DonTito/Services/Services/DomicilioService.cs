using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Request;
using Core.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Models;
using Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Services
{
    public class DomicilioService : IDomicilioService
    {
        private readonly DonTitoContext _context;
        public DomicilioService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DomicilioDtoOut>> GetDomicilio()
        {
            return await _context.Domicilio.Select(d => new DomicilioDtoOut
            {
                Id = d.Id,
                Calle = d.Calle,
                Departamento = d.Departamento,
                Numero = d.Numero,
                Piso = d.Piso,
                NombreProvincia = d.IdProvinciaNavigation.Nombre
            }).ToArrayAsync();
        }

        public async Task<DomicilioDtoOut?> GetDomicilioDtoById(int id)
        {
            return await _context.Domicilio
                .Where(d => d.Id == id)
                .Select(d => new DomicilioDtoOut
                {
                    Id = d.Id,
                    Calle = d.Calle,
                    Departamento = d.Departamento,
                    Numero = d.Numero,
                    Piso = d.Piso,
                    NombreProvincia = d.IdProvinciaNavigation.Nombre
                }).SingleOrDefaultAsync();
        }

        public async Task<Domicilio?> GetById(int id)
        {
            return await _context.Domicilio.FindAsync(id);
        }
        public async Task<Domicilio> Create(DomicilioDtoIn newDomicilioDto)
        {
            var newDomicilio = new Domicilio();

            newDomicilio.Calle = newDomicilioDto.Calle;
            newDomicilio.Departamento = newDomicilioDto.Departamento;
            newDomicilio.Numero = newDomicilioDto.Numero;
            newDomicilio.Piso = newDomicilioDto.Piso;
            newDomicilio.IdProvincia = newDomicilioDto.IdProvincia;

            _context.Domicilio.Add(newDomicilio);
            await _context.SaveChangesAsync();

            return newDomicilio;

        }


        public async Task Update(int id, DomicilioDtoIn domicilioDtoIn)
        {
            var existingDomicilio = await GetById(id);
            if (existingDomicilio is not null)
            {
                existingDomicilio.Calle = domicilioDtoIn.Calle;
                existingDomicilio.Departamento = domicilioDtoIn.Departamento;
                existingDomicilio.Numero = domicilioDtoIn.Numero;
                existingDomicilio.Piso = domicilioDtoIn.Piso;
                existingDomicilio.IdProvincia = domicilioDtoIn.IdProvincia;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Domicilio.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
