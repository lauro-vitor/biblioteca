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
            services.AddTransient<ITurmaRepositorio, TurmaRepositorio>();

            services.AddTransient<EditoraRepositorio>();
            services.AddTransient<AutorRepositorio>();
            services.AddTransient<GeneroRepositorio>();
            services.AddTransient<LivroRepositorio>();
            services.AddTransient<LivroAutorRepositorio>();
            services.AddTransient<LivroGeneroRepositorio>();

            services.AddTransient<ParentescoRepositorio>();
            services.AddTransient<AlunoRepositorio>();
            services.AddTransient<AlunoContatoRepositorio>();
        }
    }
}
