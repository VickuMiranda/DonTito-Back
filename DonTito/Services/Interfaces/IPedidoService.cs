using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoDtoOut>> GetPedido();
        Task<PedidoDtoOut?> GetPedidoDtoById(int id);
        Task<Pedido> Create(PedidoDtoIn newPedido);
        Task<Pedido?> GetById(int id);
        Task Update(int id, PedidoDtoIn pedidoDtoIn);
        Task Delete(int id);
    }
}