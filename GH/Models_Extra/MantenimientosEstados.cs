namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MantenimientosEstados
    {
        [Key]
        public int id_mantenimiento_estado { get; set; }

        public int id_mantenimiento { get; set; }

        public int id_estado { get; set; }

        [Required]
        [StringLength(100)]
        public string usuario { get; set; }

        [StringLength(250)]
        public string observaciones { get; set; }

        public DateTime fecha_estado { get; set; }

        public virtual Estados Estados { get; set; }

        public virtual Mantenimientos Mantenimientos { get; set; }
    }
}
