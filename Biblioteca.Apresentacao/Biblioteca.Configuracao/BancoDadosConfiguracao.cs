using Biblioteca.Repositorio.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Biblioteca.Configuracao
{
    internal class BancoDadosConfiguracao
    {
        public  void Configurar(IServiceCollection services)
        {
            string caminhoBanco = "../../../biblioteca-banco-dados/biblioteca-dev.db";

            if (!File.Exists(caminhoBanco))
            {
                throw new Exception("Banco de dados não encontrado!");
            }

            services.AddSqlite<BibliotecaContext>("Data Source=" + caminhoBanco);
        }
    }
}
