using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IModeloService
    {
        Task<IEnumerable<ModeloDtoOut>> GetModelo();
        Task<ModeloDtoOut?> GetModeloDtoById(int id);
        Task<Modelo> Create(ModeloDtoIn newModelo);
        Task<Modelo?> GetById(int id);
        Task<ModeloDtoOut?> GetModeloByName(string name);
        Task Update(int id, ModeloDtoIn modeloDtoIn);
        Task Delete(int id);
    }
}