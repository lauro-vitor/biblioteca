using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Biblioteca.Dominio.Entidades
{
    public class Turno
    {
        [Key]
        public Guid IdTurno { get; set; }
        public string Nome { get; set; }

        [JsonIgnore]
        public ICollection<Turma>? Turmas { get; set; } 

        public Turno()
        {
            IdTurno = Guid.Empty;
            Nome = string.Empty;
        }
    }
}
