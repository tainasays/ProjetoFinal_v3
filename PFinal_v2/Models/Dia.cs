using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models
{
    public class Dia
    {
        public int DiaId { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Código de Custo")]
        public int WbsId { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime DiaData { get; set; }
        public double Horas { get; set; }

        // Propriedade de navegação para Usuario (se aplicável)
        public Usuario? Usuario { get; set; }

        // Propriedade de navegação para Wbs
        public Wbs? Wbs { get; set; }

        
        


    }
}