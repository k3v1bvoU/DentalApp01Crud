using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoHistorials = new HashSet<PedidoHistorial>();
            Tareas = new HashSet<Tarea>();
            Transacciones = new HashSet<Transaccione>();
            Protesis = new HashSet<Protesi>(); // Agregar la colección de prótesis relacionadas
        }

        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int EstadoId { get; set; }
        public string? CreatedByUserId { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<PedidoHistorial> PedidoHistorials { get; set; }
        public virtual ICollection<Tarea> Tareas { get; set; }
        public virtual ICollection<Transaccione> Transacciones { get; set; }
        public virtual ICollection<Protesi> Protesis { get; set; } // Propiedad de navegación para prótesis
    }
}
