using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Tarea
    {
        public Tarea()
        {
            TareaHistorials = new HashSet<TareaHistorial>();
        }

        public int TareaId { get; set; }
        public int PedidoId { get; set; }
        public string Descripcion { get; set; } = null!;
        public string AssignedToUserId { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
        public virtual ICollection<TareaHistorial> TareaHistorials { get; set; }
    }
}
