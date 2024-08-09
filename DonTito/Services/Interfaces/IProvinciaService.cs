using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IProvinciaService
    {
        Task<IEnumerable<ProvinciaDtoOut>> GetProvincia();
        Task<ProvinciaDtoOut?> GetProvinciaDtoById(int id);
        Task<Provincia> Create(ProvinciaDtoIn provinciaDtoIn);
        Task<Provincia?> GetById(int id);
        Task Update(int id, ProvinciaDtoIn provinciaDtoIn);
        Task Delete(int id);
    }
}