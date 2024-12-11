using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Models.Models;
using System.Net.Http;

namespace ModeloTest
{
    public class BasedeTest
    {
        protected readonly HttpClient ClientHttp;

        protected BasedeTest()
        {
            try
            {
                var fabricaDeApp = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(servicios =>
                        {
                            // Imprimir servicios registrados (para depuración)
                            foreach (var servicio in servicios)
                            {
                                Console.WriteLine($"Servicio registrado: {servicio.ServiceType.FullName}");
                            }

                            // Remover DbContextOptions<DonTitoContext> si está registrado
                            var descriptors = servicios.Where(d => d.ServiceType == typeof(DbContextOptions<DonTitoContext>)).ToList();
                            foreach (var descriptor in descriptors)
                            {
                                servicios.Remove(descriptor);
                            }

                            // Registrar base de datos en memoria
                            servicios.AddDbContext<DonTitoContext>(options =>
                                options.UseInMemoryDatabase("BDPrueba"));
                        });

                    });

                ClientHttp = fabricaDeApp.CreateClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al configurar el entorno de pruebas: {ex.Message}");
                throw;
            }
        }
    }
}
