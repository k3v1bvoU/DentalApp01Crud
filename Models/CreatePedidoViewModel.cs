namespace dentalApp02.Models
{
    public class CreatePedidoViewModel
    {
        public string ClienteNombre { get; set; }
        public string ClienteDireccion { get; set; }
        public string ClienteTelefono { get; set; }

        public string EstadoNombre { get; set; }
        public DateTime EstadoFecha { get; set; } = DateTime.Now;

        public DateTime PedidoFecha { get; set; } = DateTime.Now;
        public string CreatedByUserId { get; set; }

        public string ProtesisNombre { get; set; }
        public string ProtesisModelo3D { get; set; }
        public bool EsModelo3D { get; set; } // Campo para el checkbox

        public decimal TransaccionMonto { get; set; }

        public string TareaDescripcion { get; set; }
        public string AssignedToUserId { get; set; }

        public string ModifiedByUserId { get; set; }
    }
}