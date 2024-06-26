using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int EstadoId { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime? Fecha { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
