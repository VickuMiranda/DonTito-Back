using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ModeloTest
{
    public class MarcaControllerTest : BasedeTest
    {
        [Fact]
        public async Task PostMarca_AgregarUna()
        {
            // Configurar contexto de prueba en memoria
            var dbContextOptions = new DbContextOptionsBuilder<DonTitoContext>()
                .UseInMemoryDatabase("DBPrueba")
                .Options;

            using (var context = new DonTitoContext(dbContextOptions))
            {
                // Asegurarse de que la base de datos esté vacía
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Arrange: Crear una nueva marca
                var marcaAgregar = new Marca
                {
                    Nombre = "MAKINA"
                };

                // Act: Realizar la solicitud HTTP POST
                var resultadoMarcaPost = await ClientHttp.PostAsJsonAsync("/api/Marca", marcaAgregar);

                // Assert: Verificar estado HTTP y contenido
                Assert.Equal(HttpStatusCode.OK, resultadoMarcaPost.StatusCode);

                // Verificar que la marca fue añadida a la base de datos
                var marcasEnDb = context.Marca.AddAsync(marcaAgregar);
                
                Assert.Equal("MAKINA", marcasEnDb.ToString());
            }
        }
    }
}
