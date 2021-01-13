namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Horas
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

        public TimeSpan? ingreso1 { get; set; }

        public TimeSpan? egreso1 { get; set; }

        public TimeSpan? ingreso2 { get; set; }

        public TimeSpan? egreso2 { get; set; }

        public TimeSpan? horas_viaje { get; set; }

        public bool? chofer { get; set; }

        public bool? pasajero { get; set; }

        public int? viaja_desde { get; set; }

        public int? viaja_hasta { get; set; }

        public int? id_coequiper1 { get; set; }

        public int? id_coequiper2 { get; set; }

        public int? id_coequiper3 { get; set; }

        public int? id_vehiculo { get; set; }

        public bool? desarraigo { get; set; }

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

        public virtual Empleados Empleados { get; set; }

        public virtual Empleados Empleados1 { get; set; }

        public virtual Empleados Empleados2 { get; set; }

        public virtual Empleados Empleados3 { get; set; }

        public virtual Lugares Lugares { get; set; }

        public virtual Lugares Lugares1 { get; set; }

        public virtual Lugares Lugares2 { get; set; }

        public virtual Vehiculos Vehiculos { get; set; }
    }
}
