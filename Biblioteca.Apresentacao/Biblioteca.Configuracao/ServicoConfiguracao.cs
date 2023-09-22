using Biblioteca.Dominio.Servico;
using Biblioteca.Servico;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Configuracao
{
    [ExcludeFromCodeCoverage]
    internal class ServicoConfiguracao
    {
        public void Configurar(IServiceCollection services)
        {
            services.AddTransient<ITurnoServico, TurnoServico>();
        }
    }
}
