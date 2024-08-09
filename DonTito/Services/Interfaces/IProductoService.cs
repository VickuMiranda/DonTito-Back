using Core.Request;
using Core.Response;
using Models.Models;

namespace Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDtoOut>> GetProducto();
        Task<ProductoDtoOut?> GetProductoDtoById(int id);
        Task<Producto> Create(ProductoDtoIn newProducto);
        Task<Producto?> GetById(int id);
        Task<IEnumerable<ProductoDtoOut>> GetProductoByModelo(string modelo);
        Task<IEnumerable<ProductoDtoOut?>> GetProductoByNombre(string nombre);
        Task<IEnumerable<ProductoDtoOut>> GetProductoByMarca(string marca);
        Task Update(int id, ProductoDtoIn productoDtoIn);
        Task Delete(int id);
    }
}