using Biblioteca.Dominio.Objetos;

namespace Biblioteca.Dominio.Entidades
{
    public class Turma
	{
		private Guid _idTurma;

		public Guid? IdTurma
		{
			get
			{
				return _idTurma;
			}

			private set
			{
				if (value == null)
				{
					_idTurma = Guid.NewGuid();
					return;
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdTurma: inválido");
				}

				_idTurma = value.Value;
			}
		}



		private Guid _idTurno;

		public Guid? IdTurno
		{
			get
			{
				return _idTurno;
			}
			private set
			{
				if (value == null)
				{
					throw new BibliotecaException("IdTurno: inválido");
				}

				if (value == Guid.Empty)
				{
					throw new BibliotecaException("IdTurno: obrigatório");
				}

				_idTurno = value.Value;
			}
		}


		private string? _nome;
		public string? Nome
		{
			get
			{
				return _nome;
			}
			private set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new BibliotecaException("Nome: obrigatório");
				}

				_nome = value;
			}
		}


		private int _periodo;

		public int Periodo
		{
			get
			{
				return _periodo;
			}

			private set
			{
				if (value <= 0)
				{
					throw new BibliotecaException("Período: inválido");
				}

				_periodo = value;
			}
		}

		public string? Sigla { get; private set; }

		public virtual Turno? Turno { get; private set; }

        public Turma()
        {
            
        }

		public Turma(Guid? idTurma, Guid? idTurno, string? nome, int periodo, string? sigla)
		{
			IdTurma = idTurma;
			IdTurno = idTurno;
			Nome = nome;
			Periodo = periodo;
			Sigla = sigla;
		}

	}
}
