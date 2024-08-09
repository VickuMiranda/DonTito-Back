using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDtoOut>> GetUsuario();
        Task<UsuarioDtoOut?> GetUsuarioDtoById(int id);
        Task<Usuario> Create(UsuarioDtoIn newUsuario);
        Task<Usuario?> GetById(int id);
        Task Update(int id, UsuarioDtoIn usuarioDtoIn);
        Task Delete(int id);
    }
}