using Biblioteca.Dominio.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio
    {
        public async Task<LivroViewModel?> ObterPorId(Guid id)
        {
            var query = ObterQueryLivroViewModel();

            return await query.FirstOrDefaultAsync(l => l.IdLivro == id);
        }
    }
}
