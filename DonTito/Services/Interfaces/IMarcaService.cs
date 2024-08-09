using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IMarcaService
    {
        Task<IEnumerable<MarcaDtoOut>> GetMarca();
        Task<MarcaDtoOut?> GetMarcaDtoById(int id);
        Task<Marca> Create(MarcaDtoIn newMarca);
        Task<Marca?> GetById(int id);
        Task Update(int id, MarcaDtoIn marcaDtoIn);
        Task Delete(int id);
    }
}