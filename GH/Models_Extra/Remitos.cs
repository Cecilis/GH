namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Remitos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_remito { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public int id_empleado { get; set; }

        [StringLength(150)]
        public string observaciones { get; set; }

        public virtual Empleados Empleados { get; set; }
    }
}
