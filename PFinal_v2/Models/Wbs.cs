using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Wbs
    {

        public int WbsId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string? Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Is chargeability? ")]
        public bool IsChargeable { get; set; }
        public string CodigoDescricao => $"{Codigo} - {Descricao}";
    }
}
