using Microsoft.EntityFrameworkCore;

namespace PFinal_v2.Data
{
    public class PFinal_v2Context : DbContext
    {
        public PFinal_v2Context (DbContextOptions<PFinal_v2Context> options)
            : base(options)
        {
        }

        public DbSet<PFinal_v2.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Departamento> Departamento { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Dia> Dia { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Wbs> Wbs { get; set; } = default!;
    }
}
