using Core.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloTest
{
    public class ProductoServiceShould
    {
        [Fact]
        public async Task CrearProductoCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            // Crear un archivo simulado (IFormFile)
            var content = "imagen de prueba";
            var fileName = "testImage.jpg";
            var contentType = "image/jpeg";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            var files = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            using var context = new DonTitoContext(options);
            var sut = new ProductoService(context);
            var productoDtoIn = new ProductoDtoIn
            {
                Nombre = "Mak",
                Precio = 100,
                Codigo = "123",
                Descripcion = "Descripción de prueba",
                IdModelo = 1
            };

            // Act
            var productoCreado = await sut.Create(productoDtoIn, files);

            // Assert
            var productoGuardado = context.Producto.FirstOrDefault();
            Assert.NotNull(productoGuardado);
            Assert.Equal("Mak", productoGuardado.Nombre);
            Assert.Equal(productoDtoIn.Precio, productoGuardado.Precio);
            Assert.Equal(productoDtoIn.Codigo, productoGuardado.Codigo);
            Assert.Equal(productoDtoIn.Descripcion, productoGuardado.Descripcion);
            Assert.Equal(productoDtoIn.IdModelo, productoGuardado.IdModelo);
            Assert.NotNull(productoGuardado.Imagen); 
            Assert.True(productoGuardado.Imagen.Length > 0);
        }
        [Fact]
        public async Task CrearProducto_SinNombre_DeberiaLanzarExcepcion()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_SinNombre")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new ProductoService(context);

            var productoDtoIn = new ProductoDtoIn
            {
                Nombre = "", // Nombre vacío
                Precio = 100,
                Codigo = "789",
                Descripcion = "Producto sin nombre",
                IdModelo = 3
            };

            var content = "imagen de prueba";
            var fileName = "testImage.jpg";
            var contentType = "image/jpeg";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            var files = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => sut.Create(productoDtoIn, files));
            Assert.Equal("El Nombre no puede estar vacío.", exception.Message);
        }

        [Fact]
        public async Task CrearProducto_SinImagen_DeberiaLanzarExcepcion()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_SinImagen")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new ProductoService(context);
            var productoDtoIn = new ProductoDtoIn
            {
                Nombre = "Producto sin imagen",
                Precio = 50,
                Codigo = "456",
                Descripcion = "Producto sin imagen",
                IdModelo = 2
            };

            IFormFile files = null; 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => sut.Create(productoDtoIn, files));
            Assert.Equal("No se han proporcionado imágenes.", exception.Message);
        }



        [Fact]
        public async Task ObtenerProductos_DeberiaRetornarListaDeProductos()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_ListarProductos")
                .Options;

            using var context = new DonTitoContext(options);

            // Agregar datos de prueba
            var modelo = new Modelo { Id = 1, Nombre = "Modelo A" };
            context.Modelo.Add(modelo);

            context.Producto.AddRange(
                new Producto
                {
                    Id = 1,
                    Nombre = "Producto 1",
                    Precio = 100,
                    Codigo = "123",
                    Descripcion = "Descripción Producto 1",
                    Imagen = new byte[] { 1, 2, 3 },
                    IdModelo = modelo.Id,
                    IdModeloNavigation = modelo
                },
                new Producto
                {
                    Id = 2,
                    Nombre = "Producto 2",
                    Precio = 200,
                    Codigo = "456",
                    Descripcion = "Descripción Producto 2",
                    Imagen = new byte[] { 4, 5, 6 },
                    IdModelo = modelo.Id,
                    IdModeloNavigation = modelo
                }
            );

            await context.SaveChangesAsync();

            var sut = new ProductoService(context);

            // Act
            var productos = await sut.GetProducto();

            // Assert
            Assert.NotNull(productos);
            Assert.Equal(2, productos.Count()); // Verifica que se obtienen dos productos

            var producto1 = productos.FirstOrDefault(p => p.Id == 1);
            var producto2 = productos.FirstOrDefault(p => p.Id == 2);

            // Verifica que los datos del producto 1 sean correctos
            Assert.NotNull(producto1);
            Assert.Equal("Producto 1", producto1.Nombre);
            Assert.Equal(100, producto1.Precio);
            Assert.Equal("123", producto1.Codigo);
            Assert.Equal("Descripción Producto 1", producto1.Descripcion);
            Assert.NotNull(producto1.Imagen);
            Assert.Equal("Modelo A", producto1.NombreModelo);

            // Verifica que los datos del producto 2 sean correctos
            Assert.NotNull(producto2);
            Assert.Equal("Producto 2", producto2.Nombre);
            Assert.Equal(200, producto2.Precio);
            Assert.Equal("456", producto2.Codigo);
            Assert.Equal("Descripción Producto 2", producto2.Descripcion);
            Assert.NotNull(producto2.Imagen);
            Assert.Equal("Modelo A", producto2.NombreModelo);
        }

    }
}
