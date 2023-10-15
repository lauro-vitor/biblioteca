using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
	public class LivroEditora
	{
		private Guid _idLivro { get; set; }
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
					throw new BibliotecaException("IdLivro: Obrigatorio");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdLivro: Invalido");
				}


				_idLivro = value.Value;
			}
		}

		private Guid _idEditora;
		public Guid? IdEditora
		{
			get
			{
				return _idEditora;
			}
			set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdEditora: Obrigatorio");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdEditora: Invalido");
				}

				_idEditora = value.Value;
			}
		}
	}
}
