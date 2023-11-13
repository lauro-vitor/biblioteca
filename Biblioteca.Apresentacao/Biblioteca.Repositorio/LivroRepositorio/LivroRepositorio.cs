using Biblioteca.Repositorio.EntityFramework;

namespace Biblioteca.Repositorio
{
    public partial class LivroRepositorio : IDisposable
    {
        private readonly BibliotecaContext _context;

        private readonly AutorRepositorio _autorRepositorio;
        private readonly EditoraRepositorio _editoraRepositorio;
        private readonly GeneroRepositorio _generoRepositorio;

        public LivroRepositorio(BibliotecaContext context)
        {
            _context = context;
            _autorRepositorio = new AutorRepositorio(context);
            _editoraRepositorio = new EditoraRepositorio(context);
            _generoRepositorio = new GeneroRepositorio(context);
        }

     
        public void Dispose()
        {
            _context?.Dispose();
            _autorRepositorio?.Dispose();
            _editoraRepositorio?.Dispose();
            _generoRepositorio?.Dispose();
        }
    }
}
