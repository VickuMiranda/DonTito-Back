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
    public class UsuarioServiceShould
    {
        [Fact]
        public void CrearUsuarioCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new UsuarioService(context);

            var usuarioDtoIn = new UsuarioDtoIn
            {
                Email = "Mak@gmail.com",
                Contrasenia = "1234"
            };

            // Act
            sut.Create(usuarioDtoIn);

            // Assert
            var usuarioCreada = context.Usuario.FirstOrDefault();
            Assert.NotNull(usuarioCreada);
            Assert.Equal("Mak@gmail.com", usuarioCreada.Email);
            Assert.Equal("1234", usuarioCreada.Password);
        }
        [Fact]
        public async Task NoCrearUsuarioConNombreVacio()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Fail")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new UsuarioService(context);

            var usuarioDtoIn = new UsuarioDtoIn
            {
                Email = "",
                Contrasenia = ""
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => sut.Create(usuarioDtoIn));

            Assert.Equal("El mail no puede estar vacío.", exception.Message);
        }
        [Fact]
        public async Task ListarTodasLosUsuariosCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_ListarUsuarios")
                .Options;

            using (var context = new DonTitoContext(options))
            {
                // Precargar datos en la base de datos en memoria
                context.Usuario.AddRange(
                    new Usuario { Id = 1, Email = "Usuario1", Password = "1234" },
                    new Usuario { Id = 2, Email = "Usuario2", Password = "1234" },
                    new Usuario { Id = 3, Email = "Usuario3", Password = "1234" }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new DonTitoContext(options))
            {
                var sut = new UsuarioService(context);

                // Act
                var resultado = await sut.GetUsuario();

                // Assert
                Assert.NotNull(resultado);
                Assert.Equal(3, resultado.Count()); // Verificar que se devuelven 3 usuarios
                Assert.Contains(resultado, m => m.Email == "Usuario1");
                Assert.Contains(resultado, m => m.Email == "Usuario2");
                Assert.Contains(resultado, m => m.Email == "Usuario3");
            }
        }
    }
}
