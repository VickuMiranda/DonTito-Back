using Core.Request;
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
    public class ModeloServiceShould
    {
        [Fact]
        public void CrearMarcaCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new ModeloService(context);

            var modeloDtoIn = new ModeloDtoIn
            {
                Nombre = "Mak",
                IdMarca = 3
            };

            // Act
            sut.Create(modeloDtoIn);

            // Assert
            var modeloCreada = context.Modelo.FirstOrDefault();
            Assert.NotNull(modeloCreada);
            Assert.Equal("Mak", modeloCreada.Nombre);
        }
        [Fact]
        public async Task NoCrearMarcaConNombreVacio()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Fail")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new ModeloService(context);

            var modeloDtoIn = new ModeloDtoIn
            {
                Nombre = "", // Nombre inválido
                IdMarca = 3
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => sut.Create(modeloDtoIn));

            Assert.Equal("El nombre de la marca no puede estar vacío.", exception.Message);
        }
        [Fact]
        public async Task ListarTodasLasMarcasCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_ListarMarcas")
                .Options;

            using (var context = new DonTitoContext(options))
            {
                // Precargar datos en la base de datos en memoria
                context.Marca.AddRange(
                    new Marca { Id = 1, Nombre = "Marca1" },
                    new Marca { Id = 2, Nombre = "Marca2" },
                    new Marca { Id = 3, Nombre = "Marca3" }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new DonTitoContext(options))
            {
                var sut = new MarcaService(context);

                // Act
                var resultado = await sut.GetMarca();

                // Assert
                Assert.NotNull(resultado);
                Assert.Equal(3, resultado.Count()); // Verificar que se devuelven 3 marcas
                Assert.Contains(resultado, m => m.Nombre == "Marca1");
                Assert.Contains(resultado, m => m.Nombre == "Marca2");
                Assert.Contains(resultado, m => m.Nombre == "Marca3");
            }
        }
    }
}
