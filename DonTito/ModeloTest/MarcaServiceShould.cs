using Xunit;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Core.Request;
using Services.Services;

namespace ModeloTest
{
    public class MarcaServiceShould
    {
        [Fact]
        public void CrearMarcaCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new MarcaService(context);

            var marcaDtoIn = new MarcaDtoIn
            {
                Nombre = "Mak"
            };

            // Act
            sut.Create(marcaDtoIn);

            // Assert
            var marcaCreada = context.Marca.FirstOrDefault();
            Assert.NotNull(marcaCreada);
            Assert.Equal("Mak", marcaCreada.Nombre);
        }
        [Fact]
        public async Task NoCrearMarcaConNombreVacio()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Fail")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new MarcaService(context);

            var marcaDtoIn = new MarcaDtoIn
            {
                Nombre = "" // Nombre inválido
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => sut.Create(marcaDtoIn));

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

        [Fact]
        public async Task ActualizarMarcaCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_UpdateMarca")
                .Options;

            using (var context = new DonTitoContext(options))
            {
                var marca = new Marca { Id = 1, Nombre = "NombreAntiguo" };
                context.Marca.Add(marca);
                await context.SaveChangesAsync();
            }

            using (var context = new DonTitoContext(options))
            {
                var sut = new MarcaService(context);
                var updatedMarcaDto = new MarcaDtoIn { Nombre = "NombreNuevo" };

                // Act
                await sut.Update(1, updatedMarcaDto);

                // Assert
                var marcaActualizada = context.Marca.FirstOrDefault(m => m.Id == 1);
                Assert.NotNull(marcaActualizada);
                Assert.Equal("NombreNuevo", marcaActualizada.Nombre);
            }
        }
    }
}

