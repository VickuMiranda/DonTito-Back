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
    public class PedidoDetalleServiceShould
    {
        [Fact]
        public async Task CrearPedidoDetalleCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_CrearPedidoDetalle")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new PedidoDetalleService(context); 

            // Crear el PedidoDetalleDtoIn
            var pedidoDetalleDtoIn = new PedidoDetalleDtoIn
            {
                Cantidad = 2,
                IdProducto = 1,
                IdPedido = 1,
                SubTotal = 400.00f
            };

            // Act
            var pedidoDetalleCreado = await sut.Create(pedidoDetalleDtoIn);

            // Assert
            Assert.NotNull(pedidoDetalleCreado); 
            Assert.Equal(2, pedidoDetalleCreado.Cantidad);
            Assert.Equal(1, pedidoDetalleCreado.IdProducto);
            Assert.Equal(1, pedidoDetalleCreado.IdPedido);
            Assert.Equal(400.00f, pedidoDetalleCreado.SubTotal);

            // Verifica que el detalle de pedido se haya guardado en la base de datos
            var detalleEnDb = await context.PedidoDetalle
                .FirstOrDefaultAsync(d => d.Id == pedidoDetalleCreado.Id);
            Assert.NotNull(detalleEnDb);
            Assert.Equal(2, detalleEnDb.Cantidad);
            Assert.Equal(1, detalleEnDb.IdProducto);
            Assert.Equal(1, detalleEnDb.IdPedido);
            Assert.Equal(400.00f, detalleEnDb.SubTotal);
        }

        [Fact]
        public async Task AsociarPedidoDetalleConPedidoCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_AsociarDetalleAPedido")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new PedidoDetalleService(context);

            var pedidoDetalleDtoIn = new PedidoDetalleDtoIn
            {
                Cantidad = 3,
                IdProducto = 2,
                IdPedido = 1,  
                SubTotal = 600.00f
            };

            var pedido = new Pedido
            {
                Id = 1,
                Total = 600.00f,
                FechaCreacion = DateTime.Now
            };
            context.Pedido.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            var pedidoDetalleCreado = await sut.Create(pedidoDetalleDtoIn);

            // Assert
            Assert.NotNull(pedidoDetalleCreado);  
            Assert.Equal(1, pedidoDetalleCreado.IdPedido);  
        }

        [Fact]
        public async Task CrearVariosPedidoDetallesCorrectamente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_VariosDetalles")
                .Options;

            using var context = new DonTitoContext(options);
            var sut = new PedidoDetalleService(context);

            
            var detalles = new List<PedidoDetalleDtoIn>
            {
                new PedidoDetalleDtoIn { Cantidad = 2, IdProducto = 1, IdPedido = 1, SubTotal = 200.00f },
                new PedidoDetalleDtoIn { Cantidad = 3, IdProducto = 2, IdPedido = 1, SubTotal = 300.00f }
            };

            
            var pedido = new Pedido
            {
                Id = 1,
                Total = 500.00f,
                FechaCreacion = DateTime.Now
            };
            context.Pedido.Add(pedido);
            await context.SaveChangesAsync();

            // Act
            foreach (var detalle in detalles)
            {
                await sut.Create(detalle);
            }

            // Assert
            var detallesCreados = await context.PedidoDetalle
                .Where(d => d.IdPedido == 1)
                .ToListAsync();

            Assert.Equal(2, detallesCreados.Count);  
            Assert.Contains(detallesCreados, d => d.IdProducto == 1 && d.Cantidad == 2);
            Assert.Contains(detallesCreados, d => d.IdProducto == 2 && d.Cantidad == 3);
        }

    }
}
