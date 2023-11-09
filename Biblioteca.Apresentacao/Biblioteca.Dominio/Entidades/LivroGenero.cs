using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
    public class LivroGenero
    {
        private Guid _idLivroGenero = Guid.Empty;
        private Guid _idLivro = Guid.Empty;
        private Guid _idGenero = Guid.Empty;
        public virtual Livro? Livro { get; set; }
        public virtual Genero? Genero { get; set; }

        public Guid? IdLivroGenero
        {
            get { return _idLivroGenero; }

            set
            {
                if (value == Guid.Empty)
                {
                    new BibliotecaException("IdLivroGenero: Inválido");
                }

                if (value == null)
                {
                    _idLivroGenero = Guid.NewGuid();
                }
                else
                {
                    _idLivroGenero = value.Value;
                }
            }
        }

        public Guid IdLivro
        {
            get
            {
                return _idLivro;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdLivro: Invalido");
                }
                _idLivro = value;
            }
        }

        public Guid IdGenero
        {
            get
            {
                return _idGenero;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdGenero: Invalido");
                }
                _idGenero = value;
            }
        }
    }
}
