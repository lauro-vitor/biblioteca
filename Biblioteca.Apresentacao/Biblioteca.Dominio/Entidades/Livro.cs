using Biblioteca.Dominio.Objetos;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
	public class Livro
	{
		private Guid _idLivro;

        private Guid _idEditora;

        private string? _titulo;

        private DateOnly _dataPublicacao;

        private int? _edicao;

        private int? _volume;

		public Editora Editora { get; set; } = new Editora();

        [Key]
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
					_idLivro = Guid.NewGuid();
					return;
				}

				if(value == Guid.Empty)
				{
					throw new BibliotecaException("IdLivro: Invalido");
				}

				_idLivro = value.Value;
			}
		}

		
		public Guid? IdEditora
		{
			get { return _idEditora; } 
			set 
			{
				if(value == null)
				{
					throw new BibliotecaException("IdEditora: Obrigatorio");
				}

				if(value == Guid.Empty)
				{
					throw new BibliotecaException("IdEditora: Invalido");
				}

				_idEditora = value.Value;
			}
		}

	
		public string? Titulo
		{
			get
			{
				return _titulo;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new BibliotecaException("Titulo: Obrigatorio");
				}

				if(value.Trim().Length <= 3)
				{
					throw new BibliotecaException("Titulo: deve possuir mais de 3 caracteres");
				}

				_titulo = value.Trim();
			}
		}

		
		public DateOnly? DataPublicacao 
		{
			get
			{
				return _dataPublicacao;
			}
			set
			{
				if(value == null)
				{
					throw new BibliotecaException("DataPublicacao: Obrigatorio");
				}

				if(value == new DateOnly())
				{
					throw new BibliotecaException("DataPublicacao: Invalido");
				}

				if(value >= DateOnly.FromDateTime(DateTime.Now))
				{
					throw new BibliotecaException("DataPublicacao: Deve ser menor que a data atual");
				}

				_dataPublicacao = value.Value;
			}
		}

		
		public int? Edicao 
		{
			get
			{
				return _edicao;
			}
			set
			{
				if(value != null && value <= 0)
				{
					throw new BibliotecaException("Edicao: edicao deve ser um numero positivo");
				}

				_edicao = value;
			}
		}

		
		public int? Volume 
		{
			get
			{
				return _volume;
			}
			set
			{
				if (value != null && value <= 0)
				{
					throw new BibliotecaException("Edicao: edicao deve ser um numero positivo");
				}

				_volume = value;
			}
		}
	}
}
