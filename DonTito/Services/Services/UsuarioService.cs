using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Request;
using Core.Response;
using Models.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DonTitoContext _context;
        public UsuarioService(DonTitoContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UsuarioDtoOut>> GetUsuario()
        {
            return await _context.Usuario.Select(u => new UsuarioDtoOut
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Dni = u.Dni,
                Telefono = u.Telefono,
                Contrasenia = u.Contrasenia,
                NombreRol = u.IdRolNavigation.Nombre,
                IdDomicilio = u.IdDomicilioNavigation.Id
            }).ToArrayAsync();
        }

        public async Task<UsuarioDtoOut?> GetUsuarioDtoById(int id)
        {
            return await _context.Usuario
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDtoOut
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido, 
                    Email = u.Email,
                    Dni = u.Dni,
                    Telefono = u.Telefono,
                    Contrasenia = u.Contrasenia,
                    NombreRol = u.IdRolNavigation.Nombre,
                    IdDomicilio = u.IdDomicilioNavigation.Id
                }).SingleOrDefaultAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }
        public async Task<Usuario> Create(UsuarioDtoIn newUsuarioDto)
        {
            var newUsuario = new Usuario();

            newUsuario.Nombre = newUsuarioDto.Nombre;
            newUsuario.Apellido = newUsuarioDto.Apellido;
            newUsuario.Email = newUsuarioDto.Email;
            newUsuario.Dni = newUsuarioDto.Dni;
            newUsuario.Telefono = newUsuarioDto.Telefono;
            newUsuario.Contrasenia = newUsuarioDto.Contrasenia;
            newUsuario.IdRol = newUsuarioDto.IdRol;
            newUsuario.IdDomicilio = newUsuarioDto.IdDomicilio;

            _context.Usuario.Add(newUsuario);
            await _context.SaveChangesAsync();

            return newUsuario;

        }


        public async Task Update(int id, UsuarioDtoIn usuarioDtoIn)
        {
            var existingUsuario = await GetById(id);
            if (existingUsuario is not null)
            {
                existingUsuario.Nombre = usuarioDtoIn.Nombre;
                existingUsuario.Apellido=usuarioDtoIn.Apellido;
                existingUsuario.Email = usuarioDtoIn.Email;
                existingUsuario.Dni = usuarioDtoIn.Dni;
                existingUsuario.Telefono = usuarioDtoIn.Telefono;
                existingUsuario.Contrasenia = usuarioDtoIn.Contrasenia;
                existingUsuario.IdRol = usuarioDtoIn.IdRol;
                existingUsuario.IdDomicilio = usuarioDtoIn.IdDomicilio;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var ToDelete = await GetById(id);

            if (ToDelete is not null)
            {
                _context.Usuario.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
