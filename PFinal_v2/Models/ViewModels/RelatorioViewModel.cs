using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models.ViewModels
{
    public class RelatorioViewModel
    {
        public int WbsId { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }

        [Display(Name = "Chargeability")]
        public string? Tipo { get; set; }
        public double? HorasTotais { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DiaData { get; set; }

    }
}
