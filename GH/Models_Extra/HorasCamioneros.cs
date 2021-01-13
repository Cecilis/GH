namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HorasCamioneros
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_empleado { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime fecha { get; set; }

        public bool? trabaja { get; set; }

        public bool? franco { get; set; }

        public int? id_causa { get; set; }

        public int? id_tractor { get; set; }

        public int? id_remolque { get; set; }

        public bool? pernocte { get; set; }

        public int? id_lugar { get; set; }

        public int? id_cliente { get; set; }

        public int? id_zona { get; set; }

        public int? id_sector { get; set; }

        public int? id_servicio { get; set; }

        [StringLength(150)]
        public string observaciones { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        [StringLength(20)]
        public string accion { get; set; }

        public DateTime? fecha_accion { get; set; }

        public virtual Causas Causas { get; set; }

        public virtual Clientes Clientes { get; set; }

        public virtual Empleados Empleados { get; set; }

        public virtual Lugares Lugares { get; set; }

        public virtual Vehiculos Vehiculos { get; set; }
    }
}
