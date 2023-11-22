using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Configuracao
{
    [ExcludeFromCodeCoverage]
    public class Configuracao
    {
        public void Configurar(IServiceCollection servies)
        {
            new BancoDadosConfiguracao().Configurar(servies);
            new ServicoConfiguracao().Configurar(servies);
        }
    }
}
