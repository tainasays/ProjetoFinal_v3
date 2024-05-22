using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;

namespace PFinal_v2.Models
{
    public class SeedData
    {
            public static void Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new PFinal_v2Context(
                    serviceProvider.GetRequiredService<
                        DbContextOptions<PFinal_v2Context>>()))
                {
                    if (context.Departamento.Any())
                    {
                        return;   // DB has been seeded
                    }
                    context.Departamento.AddRange(
                        new Departamento
                        {
                            Nome = "Recursos Humanos"
                        },
                        new Departamento
                        {
                            Nome = "Full-Stack Developer"
                        },
                        new Departamento
                        {
                            Nome = "Back-End Developer"
                        },
                        new Departamento
                        {
                            Nome = "Front-End Developer"
                        },
                        new Departamento
                        {
                            Nome = "Infraestrutura"
                        },
                        new Departamento
                        {
                            Nome = "Analista de Dados"
                        }

                    );

                    context.SaveChanges();
                }
            }


        }
    }


