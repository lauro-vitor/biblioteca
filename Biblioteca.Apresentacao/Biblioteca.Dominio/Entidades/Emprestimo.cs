using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
    public class Emprestimo
    {
        private Guid _idEmprestimo;
        private Guid _idLivro;
        private Guid _idAluno;
        private DateTime _dataEmprestimo;
        private DateTime? _dataDevolucao;
        public virtual Livro? Livro { get; set; }
        public virtual Aluno? Aluno { get; set; }

        public Guid IdEmprestimo
        {
            get
            {
                return _idEmprestimo;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdEmprestimo: Obrigatório");
                }

                _idEmprestimo = value;
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
                    throw new BibliotecaException("IdLivro: Obrigatório");
                }

                _idLivro = value;
            }
        }

        public Guid IdAluno
        {
            get
            {
                return _idAluno;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdAluno: Obrigatório");
                }
            }
        }

        public DateTime DataEmprestimo
        {
            get
            {
                return _dataEmprestimo;
            }
            set
            {
                if (value == new DateTime())
                {
                    throw new BibliotecaException("DataEmprestimo: inválida");
                }

                if (value > DateTime.Now)
                {
                    throw new BibliotecaException("DataEmprestimo: Não pode ser maior que a data atual");
                }

                _dataEmprestimo = value;
            }
        }

        public DateTime? DataDevolucao
        {
            get
            {
                return _dataDevolucao;
            }
            set
            {
                if (value == null)
                    return;

                if (value == new DateTime())
                {
                    throw new BibliotecaException("DataDevolucao: inválida");
                }

                if (value > DateTime.Now)
                {
                    throw new BibliotecaException("DataDevolucao: Não pode ser maior que a data atual");
                }

                _dataDevolucao = value;
            }
        }

        public void RealizarEmprestimo(Livro? livro, Aluno? aluno)
        {
            if (aluno == null)
                throw new BibliotecaException("Aluno é obrigatório para fazer o empréstimo");

            if (aluno.Desativado)
                throw new BibliotecaException("O Aluno foi desativado, portanto não é possível fazer o empréstimo");

            if (livro == null)
                throw new BibliotecaException("Livro é obrigatório para fazer o empréstimo");

            if (livro.QuantidadeEstoque <= 0)
                throw new BibliotecaException("Quantidade insuficiente de livros para fazer o empréstimo");

            IdEmprestimo = Guid.NewGuid();

            IdLivro = livro.IdLivro;
            Livro = livro;
            Livro.QuantidadeEstoque--;

            IdAluno = aluno.IdAluno;
            Aluno = aluno;

            DataEmprestimo = DateTime.Now;
            DataDevolucao = null;
        }

        public void RealizarDevolucao()
        {
            if (Livro == null)
                throw new BibliotecaException("Livro é obrigatório para fazer o empréstimo");

            DataDevolucao = DateTime.Now;

            Livro.QuantidadeEstoque = Livro.QuantidadeEstoque + 1;
        }

    }
}
