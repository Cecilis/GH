namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Mantenimientos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mantenimientos()
        {
            MantenimientosEstados = new HashSet<MantenimientosEstados>();
            MantenimientosRepuestos = new HashSet<MantenimientosRepuestos>();
            MantenimientosTareas = new HashSet<MantenimientosTareas>();
        }

        [Key]
        public int id_mantenimiento { get; set; }

        public DateTime fecha { get; set; }

        public int id_unidad { get; set; }

        public int? kilometros { get; set; }

        public int id_tipo_mantenimiento { get; set; }

        [Required]
        [StringLength(500)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string usuario { get; set; }

        public int id_prioridad { get; set; }

        public int? id_mantenimiento_estado { get; set; }

        public DateTime? fecha_cierre { get; set; }

        public int? kilometros_cierre { get; set; }

        public int? id_mecanico { get; set; }

        public bool activo { get; set; }

        public DateTime? fecha_baja { get; set; }

        public virtual Unidades Unidades { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosEstados> MantenimientosEstados { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosRepuestos> MantenimientosRepuestos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosTareas> MantenimientosTareas { get; set; }
    }
}
