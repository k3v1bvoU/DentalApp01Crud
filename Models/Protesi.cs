using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Protesi
    {
        public int ProtesisId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Modelo3D { get; set; }
        public int PedidoId { get; set; } // Nueva propiedad para la clave foránea

        public virtual Pedido Pedido { get; set; } = null!; // Propiedad de navegación inversa
    }
}