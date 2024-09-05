using Core.Request;
using Core.Response;
using Microsoft.AspNetCore.Http;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IImagenService
    {
        Task<ImagenDtoOut?> GetImagenById(int id);
        //Task<Imagen> Create();
        Task<int> SubirImagenes(List<IFormFile> files, int idProducto);
    }
}
