using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Dia
    {
        public int DiaId { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Código de Custo")]
        [Required(ErrorMessage = "O campo Código de custo é obrigatório")]
        public int WbsId { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "O campo Data é obrigatório")]
        public DateTime DiaData { get; set; }

        [Required(ErrorMessage = "O campo Horas é obrigatório")]
        public double Horas { get; set; }

        public Usuario? Usuario { get; set; }


    }
}