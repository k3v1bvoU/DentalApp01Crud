using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class PedidoHistorial
    {
        public int HistorialId { get; set; }
        public int PedidoId { get; set; }
        public string ModifiedByUserId { get; set; } = null!;
        public DateTime FechaModificacion { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
    }
}
