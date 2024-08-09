using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IFacturaService
    {
        Task<IEnumerable<FacturaDtoOut>> GetFactura();
        Task<FacturaDtoOut?> GetFacturaDtoById(int id);
        Task<Factura> Create(FacturaDtoIn newFactura);
        Task<Factura?> GetById(int id);
        Task Update(int id, FacturaDtoIn facturaDtoIn);
        Task Delete(int id);
    }
}