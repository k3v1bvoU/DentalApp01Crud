using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Transaccione
    {
        public int TransaccionId { get; set; }
        public int PedidoId { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
    }
}
