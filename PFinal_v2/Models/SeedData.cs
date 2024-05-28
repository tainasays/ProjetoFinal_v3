using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;
using System;
using System.Linq;

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
                // Verifica se existem departamentos
                if (!context.Departamento.Any())
                {
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

                // Verifica se existem usuários
                if (!context.Usuario.Any())
                {
                    // Obtenha alguns IDs de departamento para garantir que os usuários tenham departamentos válidos
                    var departamentoRH = context.Departamento.FirstOrDefault(d => d.Nome == "Recursos Humanos");
                    var departamentoFullStack = context.Departamento.FirstOrDefault(d => d.Nome == "Full-Stack Developer");

                    if (departamentoRH != null && departamentoFullStack != null)
                    {
                        context.Usuario.AddRange(
                            new Usuario
                            {
                                Nome = "Admin",
                                Email = "admin@email.com", 
                                DepartamentoId = departamentoRH.DepartamentoId,
                                IsAdmin = true,
                                Senha = "admin123" 
                            },
                            new Usuario
                            {
                                Nome = "Colaborador",
                                Email = "colaborador@email.com", 
                                DepartamentoId = departamentoFullStack.DepartamentoId,
                                Senha = "senha123" 
                            }
                        );

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
