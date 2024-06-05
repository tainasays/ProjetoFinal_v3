﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using PFinal_v2.Data;
using PFinal_v2.Models;
using PFinal_v2.Models.ViewModels;

namespace PFinal_v2.Controllers
{
    [Authorize(Roles = "Admin, Colaborador")]
    public class DiasController : Controller
    {
        private readonly PFinal_v2Context _context;

        public DiasController(PFinal_v2Context context)
        {
            _context = context;
        }

        // GET: Dias
        [Authorize(Roles = "Admin, Colaborador")]
        public async Task<IActionResult> Index(string mes, int quinzena)
        {
            if (!User.HasClaim(c => c.Type == "UsuarioId"))
            {
                return RedirectToAction("Login", "Conta");
            }

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

            // se 'quinzena' não é fornecida, calcula a quinzena atual com base na data atual
            if (quinzena == 0)
            {
                int diaHoje = DateTime.Now.Day;
                quinzena = diaHoje <= 15 ? 1 : 2;
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

            var wbsComHoras = wbsCadastrados
                .Where(w => diasDoUsuario.Any(d => d.WbsId == w.WbsId && d.Horas > 0))
                .ToList();

            while (wbsComHoras.Count < 4)
            {
                wbsComHoras.Add(new Wbs());
            }

            var dataAtual = DateTime.Today;

            var usuario = _context.Usuario.Where(u => u.UsuarioId == usuarioId).FirstOrDefault();

            var viewModel = new DiaFormViewModel
            {
                UsuarioId = usuarioId,
                Lancamentos = diasDoUsuario,
                DataAtual = dataAtual,
                ListaWbs = wbsComHoras,
                Quinzena = quinzena,
                Mes = mes,
                Usuario = usuario,

            };



            return View(viewModel);
        }


        // GET: Dias/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin, Colaborador")]
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
        [Authorize(Roles = "Admin, Colaborador")]
        public async Task<IActionResult> Create([Bind("DiaId,WbsId,DiaData,Horas")] Dia dia)
        {
            if (ModelState.IsValid)
            {
                // Verifica se as horas estão dentro de um intervalo válido
                if (dia.Horas <= 0 || dia.Horas > 24)
                {
                    ModelState.AddModelError("Horas", "Insira um valor válido.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // Verifica se a data escolhida está dentro do intervalo permitido
                var hoje = DateTime.Today;
                var inicioDaQuinzenaAnterior = hoje.AddDays(-(hoje.Day < 15 ? 15 : hoje.Day - 15));
                var fimDaProximaQuinzena = hoje.AddDays(hoje.Day < 15 ? 15 - hoje.Day : DateTime.DaysInMonth(hoje.Year, hoje.Month) - hoje.Day + 15);

                if (dia.DiaData < inicioDaQuinzenaAnterior || dia.DiaData > fimDaProximaQuinzena)
                {
                    ModelState.AddModelError("DiaData", "Não são permitidos lançamentos no período desejado.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }

                // Verifica se já existe um registro com o mesmo DiaId e WbsId
                if (_context.Dia.Any(d => d.DiaData.Date == dia.DiaData.Date && d.WbsId == dia.WbsId))
                {
                    ModelState.AddModelError("WbsId", "Este código de custo já está cadastrado neste registro. Alterações devem ser feitas em 'Editar'.");
                    ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
                    return View(dia);
                }


                dia.UsuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

                _context.Add(dia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");
            return View(dia);
        }


        // GET: Dias/Edit/5
        [Authorize(Roles = "Admin, Colaborador")]
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

            ViewBag.WbsList = new SelectList(_context.Wbs, "WbsId", "CodigoDescricao");


            return View(dia);


        }

        // POST: Dias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Colaborador")]
        public async Task<IActionResult> Edit(int id, [Bind("DiaId,WbsId,DiaData,Horas")] Dia dia)
        {
            if (id != dia.DiaId)
            {
                return NotFound();
            }

            if (dia.Horas <= 0 || dia.Horas > 24)
            {
                ModelState.AddModelError("Horas", "Insira um valor válido.");
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
        [Authorize(Roles = "Admin, Colaborador")]
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
        [Authorize(Roles = "Admin, Colaborador")]
        public async Task<IActionResult> Delete(int id)
        {
            var dia = await _context.Dia.FindAsync(id);
            if (dia != null)
            {
                _context.Dia.Remove(dia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Admin, Colaborador")]
        private bool DiaExists(int id)
        {
            return _context.Dia.Any(e => e.DiaId == id);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Relatorio(int? usuarioId, string mes, int? quinzena)
        {

            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Conta");
            }


            int usuarioIdLogado = int.Parse(User.FindFirst("UsuarioId").Value);

            int usuarioSelecionadoId = usuarioId ?? usuarioIdLogado;


            var usuarios = await _context.Usuario.ToListAsync();

            if (string.IsNullOrEmpty(mes))
            {
                mes = DateTime.Now.ToString("yyyy-MM");
            }


            var relatorioQuery = _context.Dia
                .Include(d => d.Wbs)
                .Where(d => d.UsuarioId == usuarioSelecionadoId)
                .GroupBy(d => new { d.Wbs.WbsId, d.Wbs.Codigo, d.Wbs.Descricao, Tipo = d.Wbs.IsChargeable ? "Sim" : "Não", d.DiaData })
                .Select(g => new RelatorioViewModel
                {
                    WbsId = g.Key.WbsId,
                    Codigo = g.Key.Codigo,
                    Descricao = g.Key.Descricao,
                    Tipo = g.Key.Tipo,
                    DiaData = g.Key.DiaData,
                    HorasTotais = g.Sum(d => d.Horas)
                });


            DateTime mesDateTime;
            if (!DateTime.TryParseExact(mes, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out mesDateTime))
            {

                throw new ArgumentException("Formato de mês inválido");
            }


            var startDate = new DateTime(mesDateTime.Year, mesDateTime.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);


            if (quinzena.HasValue)
            {
                if (quinzena == 1)
                {
                    endDate = startDate.AddDays(14);
                }
                else
                {
                    startDate = startDate.AddDays(15);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
            }

            relatorioQuery = relatorioQuery.Where(d => d.DiaData >= startDate && d.DiaData <= endDate);

            var relatorio = await relatorioQuery
                .OrderByDescending(w => w.HorasTotais)
                .ToListAsync();

            var viewModel = new RelatorioFiltroViewModel
            {
                Relatorio = relatorio,
                Usuarios = usuarios,
                UsuarioSelecionadoId = usuarioSelecionadoId,
                Mes = mes,
                Quinzena = quinzena
            };

            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RelatorioTotal(string mes, int? quinzena)
        {

            if (string.IsNullOrEmpty(mes))
            {
                mes = DateTime.Now.ToString("yyyy-MM");
            }

            DateTime mesDateTime;
            if (!DateTime.TryParseExact(mes, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out mesDateTime))
            {

                throw new ArgumentException("Formato de mês inválido");
            }

            var startDate = new DateTime(mesDateTime.Year, mesDateTime.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);


            if (quinzena.HasValue)
            {
                if (quinzena == 1)
                {
                    endDate = startDate.AddDays(14);
                }
                else
                {
                    startDate = startDate.AddDays(15);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
            }


            var relatorioQuery = _context.Dia
                .Include(d => d.Wbs)
                .Where(d => d.DiaData >= startDate && d.DiaData <= endDate)
                .GroupBy(d => new { d.Wbs.WbsId, d.Wbs.Codigo, d.Wbs.Descricao, Tipo = d.Wbs.IsChargeable ? "Sim" : "Não" })
                .Select(g => new RelatorioViewModel
                {
                    WbsId = g.Key.WbsId,
                    Codigo = g.Key.Codigo,
                    Descricao = g.Key.Descricao,
                    Tipo = g.Key.Tipo,
                    HorasTotais = g.Sum(d => d.Horas)
                });

            var relatorio = await relatorioQuery
                .OrderByDescending(w => w.HorasTotais)
                .ToListAsync();

            if (quinzena.HasValue)
            {
                if (quinzena == 1)
                {
                    endDate = startDate.AddDays(14);
                }
                else
                {
                    startDate = startDate.AddDays(15);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                }
            }

            var viewModel = new RelatorioFiltroViewModel
            {
                Relatorio = relatorio,
                Mes = mes,
                Quinzena = quinzena
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportarRelatorio(string mes, int? quinzena)
        {
            if (string.IsNullOrEmpty(mes))
            {
                mes = DateTime.Now.ToString("yyyy-MM");
            }

            DateTime mesDateTime;
            if (!DateTime.TryParseExact(mes, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out mesDateTime))
            {
                throw new ArgumentException("Formato de mês inválido");
            }

            var startDate = new DateTime(mesDateTime.Year, mesDateTime.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            string periodo;
            if (quinzena.HasValue)
            {
                if (quinzena == 1)
                {
                    endDate = startDate.AddDays(14);
                    periodo = $"Primeira quinzena de {mesDateTime.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))} de {mesDateTime.Year}";
                }
                else
                {
                    startDate = startDate.AddDays(15);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    periodo = $"Segunda quinzena de {mesDateTime.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))} de {mesDateTime.Year}";
                }
            }
            else
            {
                periodo = $"Mês de {mesDateTime.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))} de {mesDateTime.Year}";
            }

            var relatorioQuery = _context.Dia
                .Include(d => d.Wbs)
                .Where(d => d.DiaData >= startDate && d.DiaData <= endDate)
                .GroupBy(d => new { d.Wbs.WbsId, d.Wbs.Codigo, d.Wbs.Descricao, Tipo = d.Wbs.IsChargeable ? "Sim" : "Não" })
                .Select(g => new RelatorioViewModel
                {
                    WbsId = g.Key.WbsId,
                    Codigo = g.Key.Codigo,
                    Descricao = g.Key.Descricao,
                    Tipo = g.Key.Tipo,
                    HorasTotais = g.Sum(d => d.Horas)
                });

            var relatorio = await relatorioQuery
                .OrderByDescending(w => w.HorasTotais)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Relatório");

                // Período
                worksheet.Cell(1, 1).Value = "Período:";
                worksheet.Cell(1, 2).Value = periodo;

                // Cabeçalhos
                worksheet.Cell(3, 1).Value = "Código";
                worksheet.Cell(3, 2).Value = "Descrição";
                worksheet.Cell(3, 3).Value = "Chargeability";
                worksheet.Cell(3, 4).Value = "Horas Totais";

                // Dados
                for (int i = 0; i < relatorio.Count; i++)
                {
                    worksheet.Cell(i + 4, 1).Value = relatorio[i].Codigo;
                    worksheet.Cell(i + 4, 2).Value = relatorio[i].Descricao;
                    worksheet.Cell(i + 4, 3).Value = relatorio[i].Tipo;
                    worksheet.Cell(i + 4, 4).Value = relatorio[i].HorasTotais;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Relatorio.xlsx");
                }
            }
        }
    }
}
