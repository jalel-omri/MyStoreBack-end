using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class CommandsController : Controller
    {
        private readonly produitsContext _context;

        public CommandsController(produitsContext context)
        {
            _context = context;
        }

        // GET: Commands
        public async Task<IActionResult> Index()
        {
            var produitsContext = _context.Commands.Include(c => c.IdAcheteurNavigation).Include(c => c.IdProduitNavigation);
            return View(await produitsContext.ToListAsync());
        }

        // GET: Commands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commands = await _context.Commands
                .Include(c => c.IdAcheteurNavigation)
                .Include(c => c.IdProduitNavigation)
                .FirstOrDefaultAsync(m => m.IdCom == id);
            if (commands == null)
            {
                return NotFound();
            }

            return View(commands);
        }

        // GET: Commands/Create
        public IActionResult Create()
        {
            ViewData["IdAcheteur"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Descprod");
            return View();
        }

        // POST: Commands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCom,IdAcheteur,IdProduit,Quantity,Date,Livraison")] Commands commands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAcheteur"] = new SelectList(_context.Users, "Id", "Email", commands.IdAcheteur);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Descprod", commands.IdProduit);
            return View(commands);
        }

        // GET: Commands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commands = await _context.Commands.FindAsync(id);
            if (commands == null)
            {
                return NotFound();
            }
            ViewData["IdAcheteur"] = new SelectList(_context.Users, "Id", "Email", commands.IdAcheteur);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Descprod", commands.IdProduit);
            return View(commands);
        }

        // POST: Commands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCom,IdAcheteur,IdProduit,Quantity,Date,Livraison")] Commands commands)
        {
            if (id != commands.IdCom)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandsExists(commands.IdCom))
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
            ViewData["IdAcheteur"] = new SelectList(_context.Users, "Id", "Email", commands.IdAcheteur);
            ViewData["IdProduit"] = new SelectList(_context.Produits, "Id", "Descprod", commands.IdProduit);
            return View(commands);
        }

        // GET: Commands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commands = await _context.Commands
                .Include(c => c.IdAcheteurNavigation)
                .Include(c => c.IdProduitNavigation)
                .FirstOrDefaultAsync(m => m.IdCom == id);
            if (commands == null)
            {
                return NotFound();
            }

            return View(commands);
        }

        // POST: Commands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commands = await _context.Commands.FindAsync(id);
            _context.Commands.Remove(commands);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandsExists(int id)
        {
            return _context.Commands.Any(e => e.IdCom == id);
        }
    }
}
