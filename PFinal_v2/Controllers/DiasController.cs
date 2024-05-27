using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;
using PFinal_v2.Models;
using PFinal_v2.Models.ViewModels;

namespace PFinal_v2.Controllers
{
    public class DiasController : Controller
    {
        private readonly PFinal_v2Context _context;

        public DiasController(PFinal_v2Context context)
        {
            _context = context;
        }

        // GET: Dias
        public async Task<IActionResult> Index(string mes, int quinzena)
        {
            if (!User.HasClaim(c => c.Type == "UsuarioId"))
            {
                // Redireciona pra página de login caso não haja usuário logado
                return RedirectToAction("Login", "Conta");
            }

            // Apanha o UsuarioId logado
            int usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            // se 'mes' é null, a variável recebe mês atual no formato escrito abaixo
            if (string.IsNullOrEmpty(mes))
            {
                mes = DateTime.Now.ToString("yyyy-MM");
            }

            DateTime startDate;
            if (!DateTime.TryParse(mes + "-01", out startDate))
            {
                // retorna erro se a data não puder ser analisada
                return BadRequest("Data inválida");
            }

            // define primeira e segunda quinzena

            DateTime endDate;
            if (quinzena == 1)
            {
                endDate = startDate.AddDays(14);
            }
            else
            {
                startDate = startDate.AddDays(15);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }

            // busca os lançamentos do usuário no período definido
            var diasDoUsuario = await _context.Dia
                .Where(d => d.UsuarioId == usuarioId && d.DiaData >= startDate && d.DiaData <= endDate)
                .ToListAsync();


            // carrega as wbs
            var wbsCadastrados = await _context.Wbs.ToListAsync();

            // filtra as wbs que possuem horas registradas
            var wbsComHoras = wbsCadastrados
                .Where(w => diasDoUsuario.Any(d => d.WbsId == w.WbsId && d.Horas > 0))
                .ToList();

            // se as wbs com horas registradas forem menores que 4, adicionar uma nova linha
            while (wbsComHoras.Count < 4)
            {
                wbsComHoras.Add(new Wbs());
            }

            // cria a data atual, usada no Index
            var dataAtual = DateTime.Today;

            // instancia a classe DiaFormViewModel pra passar os dados necessários pra View
            var viewModel = new DiaFormViewModel
            {
                UsuarioId = usuarioId,
                Lancamentos = diasDoUsuario,
                DataAtual = dataAtual,
                ListaWbs = wbsComHoras,
                Quinzena = quinzena,
                Mes = mes
            };



            return View(viewModel);
        }

        // GET: Dias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dia = await _context.Dia
                .FirstOrDefaultAsync(m => m.DiaId == id);
            if (dia == null)
            {
                return NotFound();
            }

            return View(dia);
        }

        // GET: Dias/Create
        public IActionResult Create()
        {
            ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
            return View();
        }

        // POST: Dias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiaId,WbsId,DiaData,Horas")] Dia dia)
        {
            if (ModelState.IsValid)
            {
                // vê se a data é fim de semana
                if (dia.DiaData.DayOfWeek == DayOfWeek.Saturday || dia.DiaData.DayOfWeek == DayOfWeek.Sunday)
                {
                    // adiciona um erro ao ModelState
                    ModelState.AddModelError(string.Empty, "Não é possível adicionar registros aos fins de semana.");

                    // recarregar a lista Wbs
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // Verifica se as horas estão dentro de um intervalo válido
                if (dia.Horas <= 0 || dia.Horas > 24)
                {
                    ModelState.AddModelError("Horas", "Valor inconsistente.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // Verifica se a data escolhida está dentro do intervalo permitido
                var hoje = DateTime.Today;
                var inicioDaQuinzenaAnterior = hoje.AddDays(-(hoje.Day < 15 ? 15 : hoje.Day - 15));
                var fimDaProximaQuinzena = hoje.AddDays(hoje.Day < 15 ? 15 - hoje.Day : DateTime.DaysInMonth(hoje.Year, hoje.Month) - hoje.Day + 15);

                if (dia.DiaData < inicioDaQuinzenaAnterior || dia.DiaData > fimDaProximaQuinzena)
                {
                    ModelState.AddModelError(string.Empty, "Não são permitidos registros no período desejado.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // Verifica se já existe um registro com o mesmo DiaId e WbsId
                if (_context.Dia.Any(d => d.DiaData.Date == dia.DiaData.Date && d.WbsId == dia.WbsId))

                {
                    ModelState.AddModelError(string.Empty, "Este código de custo já está cadastrado neste registro. Alterações devem ser feitas em 'Editar'.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // atribui o UsuarioId ao ID do usuário logado
                dia.UsuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                _context.Add(dia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
            return View(dia);
        }

        // GET: Dias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                TempData["ErrorMessage"] = "Nenhum registro foi selecionado.";
                return RedirectToAction(nameof(Index));
            }

            var dia = await _context.Dia.FindAsync(id);
            if (dia == null)
            {
                return NotFound();
            }

            // configura ViewBag
            ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");

            // passa o objeto 'dia' pra view
            return View(dia);

            // Passa o objeto `dia` para a view
            return View(dia);

        }

        // POST: Dias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiaId,WbsId,DiaData,Horas")] Dia dia)
        {
            if (id != dia.DiaId)
            {
                return NotFound();
            }

            if (dia.Horas <= 0 || dia.Horas > 24)
            {
                ModelState.AddModelError("Horas", "Valor inconsistente.");
                ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                return View(dia);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dia.UsuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                    _context.Update(dia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaExists(dia.DiaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dia);
        }


        // GET: Dias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Nenhum registro foi selecionado.";
                return RedirectToAction(nameof(Index));
            }

            var dia = await _context.Dia
                .FirstOrDefaultAsync(m => m.DiaId == id);
            if (dia == null)
            {
                return NotFound();
            }

            return View(dia);
        }

        // POST: Dias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dia = await _context.Dia.FindAsync(id);
            if (dia != null)
            {
                _context.Dia.Remove(dia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiaExists(int id)
        {
            return _context.Dia.Any(e => e.DiaId == id);
        }
    }
}
