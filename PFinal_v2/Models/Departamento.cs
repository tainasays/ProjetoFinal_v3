using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Departamento
    {
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string? Nome { get; set; }


        public int QuantidadeFuncionarios { get; set; }

    
        public ICollection<Usuario>? Usuarios { get; set; }

    }
}
