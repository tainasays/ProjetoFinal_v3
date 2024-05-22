using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;

namespace PFinal_v2.Models
{
    public class UsuarioData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PFinal_v2Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PFinal_v2Context>>()))
            {

                if (context.Usuario.Any())
                {
                    return;
                }
                context.Usuario.AddRange(
                    new Usuario
                    {
                        Nome = "Ana da Silva",
                        Email = "ana@email.com",
                        DepartamentoId = 1,
                        DataContratacao = new DateTime(2024, 01, 01),
                        IsAdmin = true,
                        Senha = "Senh@123"
                    },
                    new Usuario
                    {
                        Nome = "Maria Silva",
                        Email = "maria.silva@email.com",
                        DepartamentoId = 2,
                        DataContratacao = new DateTime(2024, 02, 01),
                        IsAdmin = false,
                        Senha = "Senh@123"
                    },
                    new Usuario
                    {
                        Nome = "João Oliveira",
                        Email = "joao@email.com",
                        DepartamentoId = 3,
                        DataContratacao = new DateTime(2024, 03, 01),
                        IsAdmin = false,
                        Senha = "Senh@123"
                    },
                    new Usuario
                    {
                        Nome = "José Ferreira",
                        Email = "jferreira@email.com",
                        DepartamentoId = 3,
                        DataContratacao = new DateTime(2024, 04, 01),
                        IsAdmin = false,
                        Senha = "Senh@123"
                    }

                );
                context.SaveChanges();
            }
        }
    }
}

    