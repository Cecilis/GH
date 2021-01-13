namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Zonas
    {
        [Key]
        public int id_zona { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(100)]
        public string observaciones { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_baja { get; set; }
    }
}
