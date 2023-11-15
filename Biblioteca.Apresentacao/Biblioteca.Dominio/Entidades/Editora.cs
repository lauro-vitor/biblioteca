using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
    public class Editora
	{
		private Guid _idEditora;

        private string? _nome;

		public virtual ICollection<Livro>? Livros { get; set; } = null;

		public Guid IdEditora
		{
			get
			{
				return _idEditora;
			}
			set
			{

				if(value == Guid.Empty) 
				{
					throw new BibliotecaException("IdEditora: Invalido");
				}

				_idEditora = value;
			}
		}

		public string? Nome
		{
			get
			{
				return _nome;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new BibliotecaException("Nome: Obrigatorio");
				}

				_nome = value.Trim();
			}
		}

	}
}
