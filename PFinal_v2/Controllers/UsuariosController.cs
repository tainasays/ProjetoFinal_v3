
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFinal_v2.Data;
using PFinal_v2.Models;
using PFinal_v2.Models.ViewModels;


namespace PFinal_v2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PFinal_v2Context _context;

        public UsuariosController(PFinal_v2Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
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

            var isAdmin = User.IsInRole("Admin");

            ViewBag.IsAdmin = isAdmin;

            return View(await usuarios.ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.Departamento)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }



            return View(usuario);
        }

        [Authorize(Roles = "Admin")]
        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.DepartamentoList = new SelectList(_context.Departamento, "DepartamentoId", "Nome");

            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize]
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


        // GET: Usuarios/RedefinirSenha/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RedefinirSenha(int? id)
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

            var viewModel = new RedefinirSenhaViewModel
            {
                UsuarioId = usuario.UsuarioId
            };

            return View(viewModel);
        }





        // POST: Usuarios/RedefinirSenha

        [HttpPost("RedefinirSenha")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RedefinirSenha(RedefinirSenhaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuario.FindAsync(viewModel.UsuarioId);
                if (usuario == null)
                {
                    return NotFound();
                }

                usuario.Senha = viewModel.NovaSenha; // cripto?

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    // Logout do usuário atual
                    await HttpContext.SignOutAsync();
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

                // Redirecionar para a página de login após o logout
                return RedirectToAction("Login", "Conta");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> DadosPessoais()
        {
            string id = int.Parse(User.FindFirst("UsuarioId").Value).ToString();

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Procurar o usuário pelo ID passado como parâmetro
            var usuario = await _context.Usuario
                .Include(u => u.Departamento)
                .FirstOrDefaultAsync(m => m.UsuarioId.ToString() == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Retornar a view com os dados do usuário
            return View(usuario);
        }
    }
}