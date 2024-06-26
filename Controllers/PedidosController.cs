using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using dentalApp02.Models;
using Microsoft.Data.SqlClient;

namespace dentalApp02.Controllers
{
    public class PedidosController : Controller
    {
        private readonly PedidosDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PedidosController(PedidosDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedidosDbContext = _context.Pedidos.Include(p => p.Cliente).Include(p => p.Estado);
            return View(await pedidosDbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewBag.EstadoNombre = new SelectList(_context.Estados, "Nombre", "Nombre");
            return View(new CreatePedidoViewModel());
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePedidoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                using (var connection = _context.Database.GetDbConnection() as SqlConnection)
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            var clienteCmd = new SqlCommand("INSERT INTO Clientes (Nombre, Direccion, Telefono) OUTPUT INSERTED.ClienteID VALUES (@Nombre, @Direccion, @Telefono)", connection, transaction);
                            clienteCmd.Parameters.AddWithValue("@Nombre", model.ClienteNombre);
                            clienteCmd.Parameters.AddWithValue("@Direccion", model.ClienteDireccion);
                            clienteCmd.Parameters.AddWithValue("@Telefono", model.ClienteTelefono);
                            var clienteId = (int)await clienteCmd.ExecuteScalarAsync();

                            var estadoCmd = new SqlCommand("INSERT INTO Estados (Nombre, Fecha) OUTPUT INSERTED.EstadoID VALUES (@Nombre, @Fecha)", connection, transaction);
                            estadoCmd.Parameters.AddWithValue("@Nombre", "Iniciado");
                            estadoCmd.Parameters.AddWithValue("@Fecha", model.EstadoFecha);
                            var estadoId = (int)await estadoCmd.ExecuteScalarAsync();

                            var pedidoCmd = new SqlCommand("INSERT INTO Pedidos (Fecha, ClienteID, EstadoID, CreatedByUserId) OUTPUT INSERTED.PedidoID VALUES (@Fecha, @ClienteID, @EstadoID, @CreatedByUserId)", connection, transaction);
                            pedidoCmd.Parameters.AddWithValue("@Fecha", model.PedidoFecha);
                            pedidoCmd.Parameters.AddWithValue("@ClienteID", clienteId);
                            pedidoCmd.Parameters.AddWithValue("@EstadoID", estadoId);
                            pedidoCmd.Parameters.AddWithValue("@CreatedByUserId", currentUserId);
                            var pedidoId = (int)await pedidoCmd.ExecuteScalarAsync();

                            var protesisCmd = new SqlCommand("INSERT INTO Protesis (Nombre, Modelo3D, PedidoID) VALUES (@Nombre, @Modelo3D, @PedidoID)", connection, transaction);
                            protesisCmd.Parameters.AddWithValue("@Nombre", model.ProtesisNombre);
                            protesisCmd.Parameters.AddWithValue("@Modelo3D", model.ProtesisModelo3D ?? (object)DBNull.Value);
                            protesisCmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                            await protesisCmd.ExecuteNonQueryAsync();

                            var transaccionCmd = new SqlCommand("INSERT INTO Transacciones (PedidoID, Monto, Fecha) VALUES (@PedidoID, @Monto, @Fecha)", connection, transaction);
                            transaccionCmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                            transaccionCmd.Parameters.AddWithValue("@Monto", model.TransaccionMonto);
                            transaccionCmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                            await transaccionCmd.ExecuteNonQueryAsync();

                            var pedidoHistorialCmd = new SqlCommand("INSERT INTO PedidoHistorial (PedidoID, ModifiedByUserId, FechaModificacion) VALUES (@PedidoID, @ModifiedByUserId, @FechaModificacion)", connection, transaction);
                            pedidoHistorialCmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                            pedidoHistorialCmd.Parameters.AddWithValue("@ModifiedByUserId", currentUserId);
                            pedidoHistorialCmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                            await pedidoHistorialCmd.ExecuteNonQueryAsync();

                            var tareaCmd = new SqlCommand("INSERT INTO Tareas (PedidoID, Descripcion, AssignedToUserId, FechaInicio) OUTPUT INSERTED.TareaID VALUES (@PedidoID, @Descripcion, @AssignedToUserId, @FechaInicio)", connection, transaction);
                            tareaCmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                            tareaCmd.Parameters.AddWithValue("@Descripcion", model.TareaDescripcion);
                            tareaCmd.Parameters.AddWithValue("@AssignedToUserId", model.AssignedToUserId);
                            tareaCmd.Parameters.AddWithValue("@FechaInicio", DateTime.Now);
                            var tareaId = (int)await tareaCmd.ExecuteScalarAsync();

                            var tareaHistorialCmd = new SqlCommand("INSERT INTO TareaHistorial (TareaID, ModifiedByUserId, FechaModificacion) VALUES (@TareaID, @ModifiedByUserId, @FechaModificacion)", connection, transaction);
                            tareaHistorialCmd.Parameters.AddWithValue("@TareaID", tareaId);
                            tareaHistorialCmd.Parameters.AddWithValue("@ModifiedByUserId", currentUserId);
                            tareaHistorialCmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                            await tareaHistorialCmd.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }

            ViewBag.EstadoNombre = new SelectList(_context.Estados, "Nombre", "Nombre");
            return View(model);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            var model = new EditPedidoViewModel
            {
                PedidoId = pedido.PedidoId,
                ClienteId = pedido.ClienteId,
                EstadoId = pedido.EstadoId,
                PedidoFecha = pedido.Fecha,
                ProtesisNombre = pedido.Protesis.FirstOrDefault()?.Nombre,
                ProtesisModelo3D = pedido.Protesis.FirstOrDefault()?.Modelo3D,
                TransaccionMonto = pedido.Transacciones.FirstOrDefault()?.Monto,
                TareaDescripcion = pedido.Tareas.FirstOrDefault()?.Descripcion,
                AssignedToUserId = pedido.Tareas.FirstOrDefault()?.AssignedToUserId,
                FechaFin = pedido.Tareas.FirstOrDefault()?.FechaFin
            };

            ViewBag.EstadoNombre = new SelectList(_context.Estados, "Nombre", "Nombre", pedido.Estado.Nombre);
            return View(model);
        }

        // POST: Pedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPedidoViewModel model)
        {
            if (id != model.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                using (var connection = _context.Database.GetDbConnection() as SqlConnection)
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            var pedidoCmd = new SqlCommand("UPDATE Pedidos SET Fecha = @Fecha, ClienteID = @ClienteID, EstadoID = @EstadoID, ModifiedByUserId = @ModifiedByUserId WHERE PedidoID = @PedidoID", connection, transaction);
                            pedidoCmd.Parameters.AddWithValue("@Fecha", model.PedidoFecha);
                            pedidoCmd.Parameters.AddWithValue("@ClienteID", model.ClienteId);
                            pedidoCmd.Parameters.AddWithValue("@EstadoID", model.EstadoId);
                            pedidoCmd.Parameters.AddWithValue("@ModifiedByUserId", currentUserId);
                            pedidoCmd.Parameters.AddWithValue("@PedidoID", model.PedidoId);
                            await pedidoCmd.ExecuteNonQueryAsync();

                            var tareaCmd = new SqlCommand("UPDATE Tareas SET Descripcion = @Descripcion, AssignedToUserId = @AssignedToUserId, FechaFin = @FechaFin, ModifiedByUserId = @ModifiedByUserId WHERE TareaID = @TareaID", connection, transaction);
                            tareaCmd.Parameters.AddWithValue("@Descripcion", model.TareaDescripcion);
                            tareaCmd.Parameters.AddWithValue("@AssignedToUserId", model.AssignedToUserId);
                            tareaCmd.Parameters.AddWithValue("@FechaFin", model.FechaFin ?? (object)DBNull.Value);
                            tareaCmd.Parameters.AddWithValue("@ModifiedByUserId", currentUserId);
                            tareaCmd.Parameters.AddWithValue("@TareaID", model.TareaId);
                            await tareaCmd.ExecuteNonQueryAsync();

                            var tareaHistorialCmd = new SqlCommand("INSERT INTO TareaHistorial (TareaID, ModifiedByUserId, FechaModificacion) VALUES (@TareaID, @ModifiedByUserId, @FechaModificacion)", connection, transaction);
                            tareaHistorialCmd.Parameters.AddWithValue("@TareaID", model.TareaId);
                            tareaHistorialCmd.Parameters.AddWithValue("@ModifiedByUserId", currentUserId);
                            tareaHistorialCmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                            await tareaHistorialCmd.ExecuteNonQueryAsync();

                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }

            ViewBag.EstadoNombre = new SelectList(_context.Estados, "Nombre", "Nombre", model.EstadoNombre);
            return View(model);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedidos == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pedidos == null)
            {
                return Problem("Entity set 'PedidosDbContext.Pedidos' is null.");
            }
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return (_context.Pedidos?.Any(e => e.PedidoId == id)).GetValueOrDefault();
        }
    }
}
