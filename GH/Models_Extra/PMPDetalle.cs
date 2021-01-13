namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMPDetalle")]
    public partial class PMPDetalle
    {
        [Key]
        public int id_pmpdetalle { get; set; }

        public int id_pmp { get; set; }

        public int id_tarea { get; set; }

        public int kilometraje { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_baja { get; set; }

        public virtual PMP PMP { get; set; }
    }
}
