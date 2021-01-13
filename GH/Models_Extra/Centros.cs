namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Centros
    {
        [Key]
        public int id_centro { get; set; }

        [StringLength(100)]
        public string centro { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_desde { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_hasta { get; set; }

        public int? presupuesto { get; set; }

        public bool activo { get; set; }
    }
}
