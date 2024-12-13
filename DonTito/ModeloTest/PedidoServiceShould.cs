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
    public class PedidoServiceShould
    {
        [Fact]
        public void CrearPedidoCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new PedidoService(context);

            var pedidoDtoIn = new PedidoDtoIn
            {
                Total = 250,
                FechaCreacion = DateTime.Now
            };

            // Act
            sut.Create(pedidoDtoIn);

            // Assert
            var pedidoCreado = context.Pedido.FirstOrDefault();
            Assert.NotNull(pedidoCreado);
            Assert.True(pedidoCreado.Id > 0);
            Assert.Equal(250, pedidoCreado.Total); 
            Assert.Equal(pedidoDtoIn.FechaCreacion.Date, pedidoCreado.FechaCreacion.Date);
        }

        [Fact]
        public async Task ListarTodosLosPedidosCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_ListarPedidos")
                .Options;

            using (var context = new DonTitoContext(options))
            {
                context.Pedido.AddRange(
                    new Pedido { Total = 100.50f, FechaCreacion = DateTime.Now },
                    new Pedido { Total = 250.00f, FechaCreacion = DateTime.Now },
                    new Pedido { Total = 75.75f, FechaCreacion = DateTime.Now }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new DonTitoContext(options))
            {
                var sut = new PedidoService(context);

                // Act
                var resultado = await sut.GetPedido();

                // Assert
                Assert.NotNull(resultado);
                Assert.Equal(3, resultado.Count());
            }
        }

        [Fact]
        public async Task NoEliminarPedidoInexistente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_EliminarPedidoInexistente")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new PedidoService(context);

            // Act
            await sut.Delete(999);

            // Assert
            Assert.Empty(context.Pedido);
        }
    }
}
