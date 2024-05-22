using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string? Nome { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        public string? Email { get; set; }

        [Display(Name = "Departamento ID")]
        [Required(ErrorMessage = "O campo Departamento ID é obrigatório")]
        public int DepartamentoId { get; set; }

        [Display(Name = "Data Contratação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DataContratacao { get; set; }

        [Display(Name = "User Admin")]
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        public string? Senha { get; set; }

    }
}
