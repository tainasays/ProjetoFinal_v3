namespace PFinal_v2.Models
{
    public class Departamento
    {
        public int DepartamentoId { get; set; }
        public string? Nome { get; set; }

        public int QuantidadeFuncionarios { get; set; }

        // teste resolução de problema 1o
        public ICollection<Usuario>? Usuarios { get; set; }

    }
}
