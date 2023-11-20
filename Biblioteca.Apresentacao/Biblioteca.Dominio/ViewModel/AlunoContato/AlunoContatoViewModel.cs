namespace Biblioteca.Dominio.ViewModel.AlunoContato
{
    public class AlunoContatoViewModel
    {
        public Guid? IdContato { get; set; }

        public string? Nome { get; set; }

        public string? Telefone { get; set; }

        public string? Email { get; set; }

        public string? Observacao { get; set; }

        public Guid? IdParentesco { get; set; }

        public string? NomeParentesco { get; set; }
    }
}
