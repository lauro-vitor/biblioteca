using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio.Entidades
{
    public class Turno
    {
        public Guid? IdTurno { get; set; }
        public string? Nome { get; set; }
    }
}
