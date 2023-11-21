using Biblioteca.Dominio.Objetos;
using Biblioteca.Dominio.Servico;

namespace Biblioteca.Dominio.Entidades
{
    public class Parentesco
    {
        private Guid _idParentesco;

        private string? _nome;

        public virtual ICollection<AlunoContato>? AlunoContatos { get; set; }

        public Guid IdParentesco
        {
            get
            {
                return _idParentesco;
            }
            set
            {
                if(value == Guid.Empty)
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
    }
}
