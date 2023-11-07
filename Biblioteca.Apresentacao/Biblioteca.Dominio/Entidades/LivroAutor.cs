using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class LivroAutor
	{

		private Guid _idLivro;
        private Guid _idAutor;
        private Guid _idLivroAutor;
		public virtual Livro? Livro { get; set; }
		public virtual Autor? Autor { get; set; }

		[Key]
		public Guid? IdLivroAutor
		{
			get
			{
				return _idLivroAutor;
			}
			set
			{
                if (value == null)
                {
                    throw new BibliotecaException("IdLivroAutor: Obrigatório");
                }

                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdLivroAutor: Inválido");
                }

				_idLivroAutor = value.Value;
            }
		}

		public Guid? IdLivro
		{
			get
			{
				return _idLivro;
			}
			set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdLivro: Obrigatório");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdLivro: Inválido");
				}


				_idLivro = value.Value;
			}
		}

		public Guid? IdAutor
		{
			get
			{
				return _idAutor;
			}
			set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdAutor: Obrigatório");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdAutor: Inválido");
				}

				_idAutor = value.Value;
			}
		}
	}
}
