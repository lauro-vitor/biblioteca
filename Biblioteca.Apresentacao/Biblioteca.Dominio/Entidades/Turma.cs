namespace Biblioteca.Dominio.Entidades
{
    public class Turma
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Periodo { get; set; }    
        public string Sigla { get; set;}

        public Turma()
        {
            Id = Guid.Empty;
            Nome = string.Empty;
            Periodo = 0;
            Sigla = string.Empty;
        }

        public Turma(string nome, int periodo, string sigla)
        {
            Id = Guid.NewGuid();
            Nome = nome.Trim();
            Periodo = periodo;
            Sigla = sigla;
        }

        public void Validar()
        {
            if (this.Id == Guid.Empty)
                throw new Exception("Id da turma não pode ser vazio");

            if (string.IsNullOrEmpty(this.Nome.Trim()))
                throw new Exception("Nome da turma é obrigatório");

            if (Periodo == 0) 
                throw new Exception("Período da turma é obrigatório");
        }

    }
}
