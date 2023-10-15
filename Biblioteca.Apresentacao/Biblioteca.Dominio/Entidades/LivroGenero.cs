using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
	public class LivroGenero
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
				if(value == null)
				{
					throw new BibliotecaException("IdLivro: Obrigatorio");
				}

				if(value == Guid.Empty)
				{
					throw new BibliotecaException("IdLivro: Invalido");
				}


				_idLivro = value.Value;
			}
		}

		private Guid _idGenero;
		public Guid? IdGenero
		{
			get
			{
				return _idGenero;
			}
			set
			{
                if (value == null)
                {
					throw new BibliotecaException("IdGenero: Obrigatorio");
                }

                if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdGenero: Invalido");
				}

				_idGenero = value.Value;
			}
		}
	}
}
