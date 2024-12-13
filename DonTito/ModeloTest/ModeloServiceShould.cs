using Core.Request;
using Core.Response;
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
        public void CrearModeloCorrectamente()
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
        public async Task NoCrearModeloConNombreVacio()
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

            Assert.Equal("El nombre del modelo no puede estar vacío.", exception.Message);
        }

        [Fact]
        public async Task ListarTodasLosModelosCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_ListarModelos")
                .Options;

            using (var context = new DonTitoContext(options))
            {
                // Precargar datos relacionados en la base de datos en memoria
                context.Marca.AddRange(
                    new Marca { Id = 3, Nombre = "Marca3" },
                    new Marca { Id = 4, Nombre = "Marca4" },
                    new Marca { Id = 5, Nombre = "Marca5" }
                );

                context.Modelo.AddRange(
                    new Modelo { Id = 1, Nombre = "Modelo1", IdMarca = 3 },
                    new Modelo { Id = 2, Nombre = "Modelo2", IdMarca = 4 },
                    new Modelo { Id = 3, Nombre = "Modelo3", IdMarca = 5 }
                );

                await context.SaveChangesAsync();
            }

            using (var context = new DonTitoContext(options))
            {
                var sut = new ModeloService(context);

                // Act
                var resultado = await sut.GetModelo();

                // Assert
                Assert.NotNull(resultado);
                Assert.Equal(3, resultado.Count()); // Verificar que se devuelven 3 modelos
                Assert.Contains(resultado, m => m.Nombre == "Modelo1" && m.NombreMarca == "Marca3");
                Assert.Contains(resultado, m => m.Nombre == "Modelo2" && m.NombreMarca == "Marca4");
                Assert.Contains(resultado, m => m.Nombre == "Modelo3" && m.NombreMarca == "Marca5");
            }
        }
    
    }
}
