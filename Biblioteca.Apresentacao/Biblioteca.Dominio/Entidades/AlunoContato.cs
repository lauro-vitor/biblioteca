using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.Servico;

namespace Biblioteca.Dominio.Entidades
{
    public class AlunoContato
    {
        private Guid _idContato;
        private Guid _idAluno;
        private Guid _idParentesco;

        private string? _nome;
        private string? _telefone;
        private string? _email;
        private string? _observacao;
        public virtual Aluno? Aluno { get; set; }
        public virtual Parentesco? Parentesco { get; set; }

        public Guid IdContato
        {
            get
            {
                return _idContato;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdContato: Obrigatório");
                }

                _idContato = value;
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

                _idAluno = value;
            }
        }

        public Guid IdParentesco
        {
            get
            {
                return _idParentesco;
            }
            set
            {
                if (value == Guid.Empty)
                {
                    throw new BibliotecaException("IdParentesco: obrigatório");
                }

                _idParentesco = value;
            }
        }

        public string Nome
        {
            get
            {
                return _nome ?? string.Empty;
            }
            set
            {
                _nome = ValidacaoServico.ValidarNome("nome", value);
            }
        }

        public string Telefone
        {
            get
            {
                return _telefone ?? string.Empty;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new BibliotecaException("telefone: obrigatório");
                }

                if (!value.All(char.IsNumber))
                {
                    throw new BibliotecaException("telefone: deve ser composto de números");
                }

                if (value.Length < 10)
                {
                    throw new BibliotecaException("telefone: deve possuir ao menos 10 caractéres");
                }

                _telefone = value;
            }
        }

        public string? Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                try
                {
                    var email = new System.Net.Mail.MailAddress(value);

                    _email = email.Address;
                }
                catch
                {
                    throw new BibliotecaException("email: inválido");
                }
            }
        }

        public string? Observacao
        {
            get
            {
                return _observacao;
            }
            set
            {
                _observacao = value;
            }
        }

    }
}
