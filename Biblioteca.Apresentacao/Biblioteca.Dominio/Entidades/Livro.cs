using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel.Editora;
using Biblioteca.Dominio.ViewModel.Autor;
using Biblioteca.Dominio.ViewModel.Genero;
using Biblioteca.Dominio.ViewModel.Livro;
using System.ComponentModel.DataAnnotations;

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
                    throw new BibliotecaException("IdLivro: Invalido");
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
                    throw new BibliotecaException("IdEditora: Invalido");
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new BibliotecaException("Titulo: Obrigatorio");
                }

                if (value.Trim().Length <= 3)
                {
                    throw new BibliotecaException("Titulo: deve possuir mais de 3 caracteres");
                }

                _titulo = value.Trim();
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
                if (value == new DateOnly())
                {
                    throw new BibliotecaException("DataPublicacao: Invalido");
                }

                if (value >= DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new BibliotecaException("DataPublicacao: Deve ser menor que a data atual");
                }

                _dataPublicacao = value;
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

        public void AtribuirLivro(LivroViewModel? livroViewModel, Editora? editora)
        {
            if (livroViewModel == null)
                throw new BibliotecaException("Não foi possível atribuir as propriedades do livro");

            this.IdLivro = livroViewModel.IdLivro ?? Guid.NewGuid();
            this.Titulo = livroViewModel.Titulo?.Trim() ?? string.Empty;
            this.DataPublicacao = livroViewModel.DataPublicacao ?? new DateOnly();
            this.QuantidadeEstoque = livroViewModel.QuantidadeEstoque ?? -1;
            this.Edicao = livroViewModel.Edicao;
            this.Volume = livroViewModel.Volume;

            if (editora != null)
            {
                this.IdEditora = editora.IdEditora;
                this.Editora = editora;
            }
        }


        public LivroViewModel ConverterParaLivroViewModel(ICollection<LivroAutor>? livroAutores, ICollection<LivroGenero>? livroGeneros)
        {
            var livroViewModel = new LivroViewModel()
            {
                IdLivro = this.IdLivro,
                Titulo = this.Titulo,
                DataPublicacao = this.DataPublicacao,
                QuantidadeEstoque = this.QuantidadeEstoque,
                Edicao = this.Edicao,
                Volume = this.Volume,
                Editora = null,
                Autores = null
            };

            if (this.Editora != null)
            {
                livroViewModel.Editora = new EditoraViewModel()
                {
                    IdEditora = this.Editora?.IdEditora,
                    Nome = this.Editora?.Nome
                };
            };

            if (livroAutores != null && livroAutores.Any())
            {
                livroViewModel.Autores = new List<AutorViewModel>();

                foreach (var livroAutor in livroAutores)
                {
                    var autorViewModel = new AutorViewModel()
                    {
                        IdAutor = livroAutor?.Autor?.IdAutor,
                        Nome = livroAutor?.Autor?.Nome
                    };

                    livroViewModel.Autores.Add(autorViewModel);
                }
            }

            if (livroGeneros != null && livroGeneros.Any())
            {
                livroViewModel.Generos = new List<GeneroViewModel>();

                foreach (var livroGenero in livroGeneros)
                {
                    var generoViewModel = new GeneroViewModel()
                    {
                        IdGenero = livroGenero?.Genero?.IdGenero,
                        Nome = livroGenero?.Genero?.Nome
                    };

                    livroViewModel.Generos.Add(generoViewModel);
                }
            }

            return livroViewModel;
        }

    }
}
