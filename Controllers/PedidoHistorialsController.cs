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
    public class PedidoHistorialsController : Controller
    {
        private readonly PedidosDbContext _context;

        public PedidoHistorialsController(PedidosDbContext context)
        {
            _context = context;
        }

        // GET: PedidoHistorials
        public async Task<IActionResult> Index()
        {
            var pedidosDbContext = _context.PedidoHistorials.Include(p => p.Pedido);
            return View(await pedidosDbContext.ToListAsync());
        }

        // GET: PedidoHistorials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PedidoHistorials == null)
            {
                return NotFound();
            }

            var pedidoHistorial = await _context.PedidoHistorials
                .Include(p => p.Pedido)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (pedidoHistorial == null)
            {
                return NotFound();
            }

            return View(pedidoHistorial);
        }

        // GET: PedidoHistorials/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId");
            return View();
        }

        // POST: PedidoHistorials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistorialId,PedidoId,ModifiedByUserId,FechaModificacion")] PedidoHistorial pedidoHistorial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoHistorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", pedidoHistorial.PedidoId);
            return View(pedidoHistorial);
        }

        // GET: PedidoHistorials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PedidoHistorials == null)
            {
                return NotFound();
            }

            var pedidoHistorial = await _context.PedidoHistorials.FindAsync(id);
            if (pedidoHistorial == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", pedidoHistorial.PedidoId);
            return View(pedidoHistorial);
        }

        // POST: PedidoHistorials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistorialId,PedidoId,ModifiedByUserId,FechaModificacion")] PedidoHistorial pedidoHistorial)
        {
            if (id != pedidoHistorial.HistorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoHistorial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoHistorialExists(pedidoHistorial.HistorialId))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", pedidoHistorial.PedidoId);
            return View(pedidoHistorial);
        }

        // GET: PedidoHistorials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PedidoHistorials == null)
            {
                return NotFound();
            }

            var pedidoHistorial = await _context.PedidoHistorials
                .Include(p => p.Pedido)
                .FirstOrDefaultAsync(m => m.HistorialId == id);
            if (pedidoHistorial == null)
            {
                return NotFound();
            }

            return View(pedidoHistorial);
        }

        // POST: PedidoHistorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PedidoHistorials == null)
            {
                return Problem("Entity set 'PedidosDbContext.PedidoHistorials'  is null.");
            }
            var pedidoHistorial = await _context.PedidoHistorials.FindAsync(id);
            if (pedidoHistorial != null)
            {
                _context.PedidoHistorials.Remove(pedidoHistorial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoHistorialExists(int id)
        {
          return (_context.PedidoHistorials?.Any(e => e.HistorialId == id)).GetValueOrDefault();
        }
    }
}
