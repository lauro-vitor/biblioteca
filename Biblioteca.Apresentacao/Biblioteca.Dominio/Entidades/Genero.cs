using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Genero;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
    public class Genero
	{
		private Guid _idGenero = Guid.Empty;

        private string _nome = string.Empty;

		public virtual ICollection<LivroGenero>? LivroGeneros { get; set; }

        public Genero() { }

        public Genero(GeneroViewModel generoViewModel)
        {
			IdGenero = generoViewModel.IdGenero ?? Guid.Empty;
			Nome = generoViewModel.Nome ?? string.Empty;
        }

        [Key]
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


		public string Nome 
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
