using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Dominio.ViewModel.Livro;
using System.ComponentModel.DataAnnotations;
using Biblioteca.Dominio.Servico;

namespace Biblioteca.Dominio.Entidades
{
    public class Livro
    {
        private Guid _idLivro = Guid.Empty;

        private Guid _idEditora = Guid.Empty;

        private string _titulo = string.Empty;

        private DateOnly _dataPublicacao = new DateOnly();

        private int _quantidadeEstoque = 0;

        private int? _edicao;

        private int? _volume;

        public virtual Editora? Editora { get; set; } = null;

        public virtual ICollection<LivroAutor>? LivroAutores { get; set; } = null;

        public virtual ICollection<LivroGenero>? LivroGeneros { get; set; } = null;

        public virtual ICollection<Emprestimo>? Emprestimos { get; set; } = null;

        public Livro() { }

        [Key]
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
                    throw new BibliotecaException("IdLivro: obrigatório");
                }

                _idLivro = value;
            }
        }

        public Guid IdEditora
        {
            get
            {
                return _idEditora;
            }

            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdEditora: obrigatório");
                }

                _idEditora = value;
            }
        }


        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                _titulo = ValidacaoServico.ValidarNome("titulo", value);
            }
        }


        public DateOnly DataPublicacao
        {
            get
            {
                return _dataPublicacao;
            }
            set
            {
                _dataPublicacao = ValidacaoServico.ValidarData("dataPublicacao", value);
            }
        }

        public int QuantidadeEstoque
        {
            get
            {
                return _quantidadeEstoque;
            }
            set
            {
                if (value < 0)
                    throw new BibliotecaException("QuantidadeEstoque: Quantidade de Livros no estoque inválida");

                _quantidadeEstoque = value;
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
                if (value != null && value <= 0)
                {
                    throw new BibliotecaException("Edicao: edição deve ser um numero positivo");
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
                    throw new BibliotecaException("Volume: volume deve ser um numero positivo");
                }

                _volume = value;
            }
        }
    }
}
