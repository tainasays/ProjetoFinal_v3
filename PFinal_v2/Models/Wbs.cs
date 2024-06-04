using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Wbs
    {

        public int WbsId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo Código é obrigatório")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "O código deve ter no mínimo 4 e no máximo 10 caracteres")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "O código deve conter apenas letras e números.")]
        public string? Codigo { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        public string? Descricao { get; set; }

        [Display(Name = "Chargeability? ")]
        public bool IsChargeable { get; set; }
        public string CodigoDescricao => $"{Codigo} - {Descricao}";

        // Lista de Dias associados
        public ICollection<Dia>? Dias { get; set; }
    }
}
