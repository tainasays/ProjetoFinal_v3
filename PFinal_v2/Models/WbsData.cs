using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;

namespace PFinal_v2.Models
{
    public class WbsData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PFinal_v2Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PFinal_v2Context>>()))
            {
                // Look for any movies.
                if (context.Wbs.Any())
                {
                    return;   // DB has been seeded
                }
                context.Wbs.AddRange(
                    new Wbs
                    {
                        
                        Codigo = "WBS0912010",
                        Descricao = "Férias",
                        
                    },
                    new Wbs
                    {
                        Codigo = "WBS0912009",
                        Descricao = "Day-off",
                    },
                    new Wbs
                    {
                        Codigo = "WBS0912008",
                        Descricao = "Sem tarefa",
                    },
                    new Wbs
                    {
                        Codigo = "WBS0912007",
                        Descricao = "Implementação",
                    },
                    new Wbs
                    {
                        Codigo = "WBS09120087",
                        Descricao = "Desenvolvimento",
                    }
                );
                context.SaveChanges();
            }



        }
    }
}

  