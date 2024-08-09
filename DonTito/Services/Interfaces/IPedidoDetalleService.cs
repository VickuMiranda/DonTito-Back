using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IPedidoDetalleService
    {
        Task<IEnumerable<PedidoDetalleDtoOut>> GetPedidoDetalle();
        Task<PedidoDetalleDtoOut?> GetPedidoDetalleDtoById(int id);
        Task<PedidoDetalle> Create(PedidoDetalleDtoIn newPedidoDetalle);
        Task<PedidoDetalle?> GetById(int id);
        Task Update(int id, PedidoDetalleDtoIn pedidoDetalleDtoIn);
        Task Delete(int id);
    }
}