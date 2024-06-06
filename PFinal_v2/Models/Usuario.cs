using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace PFinal_v2.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string? Nome { get; set; }

        [Display(Name = "E-mail:")]
        [Required(ErrorMessage = "O campo E-mail é obrigatório")]
        [RegularExpression(@"^[a-zA-Z]+(([\'\,\.\- ][a-zA-Z ])?[a-zA-Z]*)*\s+<(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})>$|^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$", ErrorMessage = "Insira um e-mail válido")]
        public string? Email { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "O campo Departamento é obrigatório")]
        public int DepartamentoId { get; set; }

        [Display(Name = "Data de Contratação")]
        [Required(ErrorMessage = "O campo Data de Contratação é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DataContratacao { get; set; }

        [Display(Name = "Administrador")]
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "A senha deve ter entre 5 e 10 caracteres")]
        public string? Senha { get; set; }

        [Display(Name = "Local de Trabalho")]
        [Required(ErrorMessage = "O campo Local de Trabalho é obrigatório")]
        public LocalTrabalhoLista? LocalTrabalho { get; set; }

        public ICollection<Dia>? Dias { get; set; }

        public Departamento? Departamento { get; set; }

    }

    public enum LocalTrabalhoLista
    {
        [Display(Name = "Recife - Brasil")]
        RE,

        [Display(Name = "São Paulo - Brasil")]
        SP,
        [Display(Name = "Nova Lima - Brasil")]
        NL
    }


}

