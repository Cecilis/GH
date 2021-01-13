namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsuariosEmpleados
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_empleado { get; set; }

        public virtual Empleados Empleados { get; set; }
    }
}
