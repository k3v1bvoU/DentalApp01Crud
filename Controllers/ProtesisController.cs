using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dentalApp02.Models;

namespace dentalApp02.Controllers
{
    public class ProtesisController : Controller
    {
        private readonly PedidosDbContext _context;

        public ProtesisController(PedidosDbContext context)
        {
            _context = context;
        }

        // GET: Protesis
        public async Task<IActionResult> Index()
        {
              return _context.Proteses != null ? 
                          View(await _context.Proteses.ToListAsync()) :
                          Problem("Entity set 'PedidosDbContext.Proteses'  is null.");
        }

        // GET: Protesis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proteses == null)
            {
                return NotFound();
            }

            var protesi = await _context.Proteses
                .FirstOrDefaultAsync(m => m.ProtesisId == id);
            if (protesi == null)
            {
                return NotFound();
            }

            return View(protesi);
        }

        // GET: Protesis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Protesis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProtesisId,Nombre,Modelo3D")] Protesi protesi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(protesi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(protesi);
        }

        // GET: Protesis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proteses == null)
            {
                return NotFound();
            }

            var protesi = await _context.Proteses.FindAsync(id);
            if (protesi == null)
            {
                return NotFound();
            }
            return View(protesi);
        }

        // POST: Protesis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProtesisId,Nombre,Modelo3D")] Protesi protesi)
        {
            if (id != protesi.ProtesisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(protesi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProtesiExists(protesi.ProtesisId))
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
            return View(protesi);
        }

        // GET: Protesis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proteses == null)
            {
                return NotFound();
            }

            var protesi = await _context.Proteses
                .FirstOrDefaultAsync(m => m.ProtesisId == id);
            if (protesi == null)
            {
                return NotFound();
            }

            return View(protesi);
        }

        // POST: Protesis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proteses == null)
            {
                return Problem("Entity set 'PedidosDbContext.Proteses'  is null.");
            }
            var protesi = await _context.Proteses.FindAsync(id);
            if (protesi != null)
            {
                _context.Proteses.Remove(protesi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProtesiExists(int id)
        {
          return (_context.Proteses?.Any(e => e.ProtesisId == id)).GetValueOrDefault();
        }
    }
}
