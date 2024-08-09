using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IDomicilioService
    {
        Task<IEnumerable<DomicilioDtoOut>> GetDomicilio();
        Task<DomicilioDtoOut?> GetDomicilioDtoById(int id);
        Task<Domicilio> Create(DomicilioDtoIn newDomicilio);
        Task<Domicilio?> GetById(int id);
        Task Update(int id, DomicilioDtoIn domicilioDtoIn);
        Task Delete(int id);
    }
}