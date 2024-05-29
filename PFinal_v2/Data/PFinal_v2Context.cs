using Microsoft.EntityFrameworkCore;
using PFinal_v2.Models;

namespace PFinal_v2.Data
{
    public class PFinal_v2Context : DbContext
    {
        public PFinal_v2Context(DbContextOptions<PFinal_v2Context> options)
            : base(options)
        {
        }

        public DbSet<PFinal_v2.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Departamento> Departamento { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Dia> Dia { get; set; } = default!;
        public DbSet<PFinal_v2.Models.Wbs> Wbs { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Departamento)
                .WithMany(d => d.Usuarios)
                .HasForeignKey(u => u.DepartamentoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}