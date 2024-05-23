namespace PFinal_v2.Models
{
    public class Wbs
    {
        public int WbsId { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public bool IsChargeable { get; set; }
        public string CodigoDescricao => $"{Codigo} - {Descricao}";
    }
}
