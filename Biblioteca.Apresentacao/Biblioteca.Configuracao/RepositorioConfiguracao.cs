using Biblioteca.Dominio.Repositorio;
using Biblioteca.Repositorio;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Configuracao
{
    [ExcludeFromCodeCoverage]
    internal class RepositorioConfiguracao
    {
        public void Configurar(IServiceCollection services)
        {
            services.AddTransient<ITurnoRepositorio, TurnoRepositorio>();
        }
    }
}
