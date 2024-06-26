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
    public class TareaHistorialsController : Controller
    {
        private readonly PedidosDbContext _context;

        public TareaHistorialsController(PedidosDbContext context)
        {
            _context = context;
        }

        // GET: TareaHistorials
        public async Task<IActionResult> Index()
        {
            var pedidosDbContext = _context.TareaHistorials.Include(t => t.Tarea);
            return View(await pedidosDbContext.ToListAsync());
        }

        // GET: TareaHistorials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TareaHistorials == null)
            {
                return NotFound();
            }

            var tareaHistorial = await _context.TareaHistorials
                .Include(t => t.Tarea)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (tareaHistorial == null)
            {
                return NotFound();
            }

            return View(tareaHistorial);
        }

        // GET: TareaHistorials/Create
        public IActionResult Create()
        {
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId");
            return View();
        }

        // POST: TareaHistorials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistorialId,TareaId,ModifiedByUserId,FechaModificacion")] TareaHistorial tareaHistorial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tareaHistorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", tareaHistorial.TareaId);
            return View(tareaHistorial);
        }

        // GET: TareaHistorials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TareaHistorials == null)
            {
                return NotFound();
            }

            var tareaHistorial = await _context.TareaHistorials.FindAsync(id);
            if (tareaHistorial == null)
            {
                return NotFound();
            }
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", tareaHistorial.TareaId);
            return View(tareaHistorial);
        }

        // POST: TareaHistorials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistorialId,TareaId,ModifiedByUserId,FechaModificacion")] TareaHistorial tareaHistorial)
        {
            if (id != tareaHistorial.HistorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tareaHistorial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaHistorialExists(tareaHistorial.HistorialId))
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
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", tareaHistorial.TareaId);
            return View(tareaHistorial);
        }

        // GET: TareaHistorials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TareaHistorials == null)
            {
                return NotFound();
            }

            var tareaHistorial = await _context.TareaHistorials
                .Include(t => t.Tarea)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (tareaHistorial == null)
            {
                return NotFound();
            }

            return View(tareaHistorial);
        }

        // POST: TareaHistorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TareaHistorials == null)
            {
                return Problem("Entity set 'PedidosDbContext.TareaHistorials'  is null.");
            }
            var tareaHistorial = await _context.TareaHistorials.FindAsync(id);
            if (tareaHistorial != null)
            {
                _context.TareaHistorials.Remove(tareaHistorial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TareaHistorialExists(int id)
        {
          return (_context.TareaHistorials?.Any(e => e.HistorialId == id)).GetValueOrDefault();
        }
    }
}
