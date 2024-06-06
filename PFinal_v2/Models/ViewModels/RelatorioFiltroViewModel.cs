using System.ComponentModel.DataAnnotations;

namespace PFinal_v2.Models.ViewModels
{
    public class RelatorioFiltroViewModel
    {
        public IEnumerable<RelatorioViewModel>? Relatorio { get; set; }
        public IEnumerable<Usuario>? Usuarios { get; set; }
        public int UsuarioSelecionadoId { get; set; }

        [DataType(DataType.Date)]
        public string? Mes { get; set; }
        public int? Quinzena { get; set; }
        public string? SearchString { get; set; }

        public string? TipoAgrupamento { get; set; }
    }
}
