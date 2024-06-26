namespace dentalApp02.Controllers
{
    public class EditPedidoViewModel
    {
        public int PedidoId { get; set; }
        public DateTime PedidoFecha { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }
        public int EstadoId { get; set; }

        // Cliente Fields
        public string ClienteNombre { get; set; }
        public string ClienteDireccion { get; set; }
        public string ClienteTelefono { get; set; }

        // Estado Fields
        public string EstadoNombre { get; set; }
        public DateTime EstadoFecha { get; set; } = DateTime.Now;

        // Protesis Fields
        public string ProtesisNombre { get; set; }
        public string ProtesisModelo3D { get; set; }

        // Transaccion Fields
        public decimal TransaccionMonto { get; set; }

        // Tarea Fields
        public string TareaDescripcion { get; set; }
        public string AssignedToUserId { get; set; }

        // These fields are handled internally
        public string ModifiedByUserId { get; set; }
    }
}