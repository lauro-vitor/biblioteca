using Biblioteca.Dominio.Enums;
using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.Servico;

namespace Biblioteca.Dominio.Entidades
{
    public class Aluno
    {
        private Guid _idAulno;
        private string? _nome;
        private string? _matricula;
        private DateOnly _dataNascimento;
        private Sexo _sexo;
        private bool _desativado;
        public virtual ICollection<AlunoContato>? AlunoContatos { get; set; }

        public Guid IdAluno
        {
            get
            {
                return _idAulno;
            }
            set
            {
                _idAulno = ValidacaoServico.ValidarId("idAluno", value);
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
                _nome = ValidacaoServico.ValidarNome("nome",value);
            }
        }

        public string Matricula
        {
            get
            {
                return _matricula ?? string.Empty;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new BibliotecaException("matricula: Obrigatório");
                }

                _matricula = value;
            }
        }

        public DateOnly DataNascimento
        {
            get
            {
                return _dataNascimento;
            }
            set
            {
                _dataNascimento = ValidacaoServico.ValidarData("dataNascimento", value);
            }
        }

        public Sexo? Sexo
        {
            get
            {
                return _sexo;
            }
            set
            {
                if (value == null)
                    throw new BibliotecaException("sexo: Obrigatório");

                _sexo = value.Value;
            }
        }

        public bool Desativado
        {
            get
            {
                return _desativado;
            }
            set
            {
                _desativado = value;    
            }
        }
    }
}
