using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.ViewModel;
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
                    throw new BibliotecaException("QuantidadeEstoque: Quantidade de Livros inválida");

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

        public void AtribuirLivro(LivroViewModel livroViewModel, Editora? editora,  ICollection<Autor>? autores, ICollection<Genero>? generos)
        {
            this.IdLivro = livroViewModel.IdLivro ?? Guid.NewGuid();
            this.Titulo = livroViewModel.Titulo?.Trim() ?? string.Empty;
            this.DataPublicacao = livroViewModel.DataPublicacao ?? new DateOnly();
            this.QuantidadeEstoque = livroViewModel.QuantidadeEstoque ?? -1;
            this.Edicao = livroViewModel.Edicao;
            this.Volume = livroViewModel.Volume;

            if (editora != null)
            {
                this.IdEditora = editora.IdEditora ?? Guid.Empty;
                this.Editora = editora;
            }

            if (autores != null && autores.Any())
            {
                this.LivroAutores = new List<LivroAutor>();

                foreach (var autor in autores)
                {
                    var livroAutor = new LivroAutor
                    {
                        IdLivro = this.IdLivro,
                        IdAutor = autor.IdAutor,
                        Livro = this,
                        Autor = autor,
                    };

                    this.LivroAutores.Add(livroAutor);
                }
            }

            if(generos != null && generos.Any())
            {
                this.LivroGeneros = new List<LivroGenero>();

                foreach(var genero in generos)
                {
                    var livroGenero = new LivroGenero()
                    {
                        IdLivro = this.IdLivro,
                        IdGenero = genero.IdGenero,
                        Livro = this,
                        Genero = genero
                    };
                    this.LivroGeneros.Add(livroGenero);
                }
            }
        }

        public LivroViewModel ConverterParaLivroViewModel()
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

            if (this.LivroAutores != null && this.LivroAutores.Any())
            {
                livroViewModel.Autores = new List<AutorViewModel>();

                foreach (var livroAutor in this.LivroAutores)
                {
                    var autorViewModel = new AutorViewModel()
                    {
                        IdAutor = livroAutor?.Autor?.IdAutor,
                        Nome = livroAutor?.Autor?.Nome
                    };

                    livroViewModel.Autores.Add(autorViewModel);
                }
            }

            return livroViewModel;
        }
    }
}
