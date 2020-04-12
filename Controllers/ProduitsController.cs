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
    public class ProduitsController : Controller
    {
        private readonly produitsContext _context;

        public ProduitsController(produitsContext context)
        {
            _context = context;
        }

        // GET: Produits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produits.ToListAsync());
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produits == null)
            {
                return NotFound();
            }

            return View(produits);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Improd,Namprod,Descprod,Priprod,Discprod,Stock")] Produits produits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produits);
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Produits.FindAsync(id);
            if (produits == null)
            {
                return NotFound();
            }
            return View(produits);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Improd,Namprod,Descprod,Priprod,Discprod,Stock")] Produits produits)
        {
            if (id != produits.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitsExists(produits.Id))
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
            return View(produits);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produits = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produits == null)
            {
                return NotFound();
            }

            return View(produits);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produits = await _context.Produits.FindAsync(id);
            _context.Produits.Remove(produits);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitsExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }
    }
}
