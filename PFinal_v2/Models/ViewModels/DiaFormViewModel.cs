namespace PFinal_v2.Models.ViewModels
{
    public class DiaFormViewModel
    {
        public int UsuarioId { get; set; }
        public List<Dia>? Lancamentos { get; set; }
        public DateTime DataAtual { get; set; }
        public List<Wbs>? ListaWbs { get; set; }
        public int Quinzena { get; set; }
        public string? Mes { get; set; }


        public string? MensagemErro { get; set;}
    }
}
