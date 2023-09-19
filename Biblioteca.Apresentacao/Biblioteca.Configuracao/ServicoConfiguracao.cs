using Biblioteca.Dominio.Servico;
using Biblioteca.Servico;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Configuracao
{
    internal class ServicoConfiguracao
    {
        public void Configurar(IServiceCollection services)
        {
            services.AddTransient<ITurnoServico, TurnoServico>();
        }
    }
}
