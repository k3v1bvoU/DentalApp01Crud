using System;
using System.Collections.Generic;

namespace dentalApp02.Models
{
    public partial class TareaHistorial
    {
        public int HistorialId { get; set; }
        public int TareaId { get; set; }
        public string ModifiedByUserId { get; set; } = null!;
        public DateTime FechaModificacion { get; set; }

        public virtual Tarea Tarea { get; set; } = null!;
    }
}
