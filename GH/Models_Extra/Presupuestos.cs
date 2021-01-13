namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Presupuestos
    {
        [Key]
        public int id_presupuesto { get; set; }

        public int id_nota { get; set; }

        public int id_proveedor { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? total { get; set; }

        [StringLength(250)]
        public string observaciones { get; set; }

        [Required]
        [StringLength(100)]
        public string archivo { get; set; }

        public DateTime fecha_alta { get; set; }

        public bool? seleccionado { get; set; }

        public virtual Notas Notas { get; set; }

        public virtual Proveedores Proveedores { get; set; }
    }
}
