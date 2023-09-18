using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Configuracao
{
    public class Configuracao
    {
        public void Configurar(IServiceCollection servies)
        {
            new BancoDadosConfiguracao().Configurar(servies);
            new RepositorioConfiguracao().Configurar(servies);
            new ServicoConfiguracao().Configurar(servies);
        }
    }
}
