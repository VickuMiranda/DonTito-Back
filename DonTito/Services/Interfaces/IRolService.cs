using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<RolDtoOut>> GetRol();
        Task<RolDtoOut?> GetRolDtoById(int id);
        Task<Rol> Create(RolDtoIn newRol);
        Task<Rol?> GetById(int id);
        Task Update(int id, RolDtoIn rolDtoIn);
        Task Delete(int id);
    }
}