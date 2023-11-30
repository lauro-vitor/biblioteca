namespace Biblioteca.Dominio.ViewModel.AlunoVM
{
    public class AlunoParametroViewModel
    {
        public DateTime? DataNascimentoInicio { get; set; }

        public DateTime? DataNascimentoFim { get; set; }

        public int? Sexo { get; set; }

        public bool? SomenteAtivos { get; set; }


        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public string? SortProp { get; set; }

        public string? SortDirection { get; set; }


        private string? _nome = null;

        public string? Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _nome = value.ToLower().Trim();
            }
        }

        private string? _matricula = null;

        public string? Matricula 
        {
            get
            {
                return _matricula;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _matricula = value.ToLower().Trim();
            }
        }

      
    }
}
