using Biblioteca.Repositorio.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteca.Configuracao
{
    [ExcludeFromCodeCoverage]
    internal class BancoDadosConfiguracao
    {
        public  void Configurar(IServiceCollection services)
        {
            string caminhoBanco = "./biblioteca-dev.db";

            if (!File.Exists(caminhoBanco))
            {
                File.Create(caminhoBanco);
            }

            services.AddSqlite<BibliotecaContext>("Data Source=" + caminhoBanco);
        }
    }
}
