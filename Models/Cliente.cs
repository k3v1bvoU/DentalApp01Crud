﻿using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
