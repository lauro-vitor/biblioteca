using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Configuracao
{
    internal class RepositorioConfiguracao
    {
        public void Configurar(IServiceCollection services)
        {
            services.AddTransient<ITurnoRepositorio, TurnoRepositorio>();
        }
    }
}
