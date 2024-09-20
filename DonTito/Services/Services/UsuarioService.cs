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
                Email = u.Email,
                Password = u.Password
            }).ToArrayAsync();
        }

        public async Task<UsuarioDtoOut?> GetUsuarioDtoById(int id)
        {
            return await _context.Usuario
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDtoOut
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password
                }).SingleOrDefaultAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }
        public async Task<Usuario> Create(UsuarioDtoIn newUsuarioDto)
        {
            var newUsuario = new Usuario();

            newUsuario.Email = newUsuarioDto.Email;
            newUsuario.Password = newUsuarioDto.Contrasenia;

            _context.Usuario.Add(newUsuario);
            await _context.SaveChangesAsync();

            return newUsuario;

        }


        public async Task Update(int id, UsuarioDtoIn usuarioDtoIn)
        {
            var existingUsuario = await GetById(id);
            if (existingUsuario is not null)
            {
                existingUsuario.Email = usuarioDtoIn.Email;
                existingUsuario.Password = usuarioDtoIn.Contrasenia;
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
