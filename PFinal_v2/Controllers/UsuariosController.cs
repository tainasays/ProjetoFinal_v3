
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;
using PFinal_v2.Models;


namespace PFinal_v2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PFinal_v2Context _context;

        public UsuariosController(PFinal_v2Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    if (_context.Usuario == null)
        //    {
        //        return Problem("Entidate sem Dados.. Null");
        //    }

        //    var usuarios = from u in _context.Usuario select u;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        usuarios = usuarios.Where(s => s.Nome!.Contains(searchString));

        //    }

        //    return View(await usuarios.ToListAsync());
        //}


        // ---------- adicionei dps:
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entidade sem dados.. Null");
            }

            IQueryable<Usuario> usuarios = _context.Usuario; // Inicializa a consulta sem o Include

            if (!string.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(s => s.Nome!.Contains(searchString));
            }

            // Aplica o Include separadamente
            usuarios = usuarios.Include(u => u.Departamento);

            usuarios = usuarios.Include(u => u.Departamento);

            return View(await usuarios.ToListAsync());
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.DepartamentoList = new SelectList(_context.Departamento, "DepartamentoId", "Nome");

            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nome,Email,DepartamentoId,DataContratacao,IsAdmin,Senha,LocalTrabalho")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartamentoList = new SelectList(_context.Departamento, "DepartamentoId", "Nome");

            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.DepartamentoList = new SelectList(_context.Departamento, "DepartamentoId", "Nome");
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nome,Email,DepartamentoId,DataContratacao,IsAdmin,Senha,LocalTrabalho")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            ViewBag.DepartamentoList = new SelectList(_context.Departamento, "DepartamentoId", "Nome");
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioId == id);
        }



        [HttpGet]
        public async Task<IActionResult> Localizacao(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Localizacao(int id, string localTrabalho)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (Enum.TryParse(localTrabalho, out LocalTrabalhoLista parsedLocalTrabalho))
            {
                usuario.LocalTrabalho = parsedLocalTrabalho;
            }
            else
            {
                ModelState.AddModelError(nameof(usuario.LocalTrabalho), "Local de trabalho inválido.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Dias");
            }
            return View(usuario);
        }








    }
}