namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Repuestos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Repuestos()
        {
            MantenimientosRepuestos = new HashSet<MantenimientosRepuestos>();
        }

        [Key]
        public int id_repuesto { get; set; }

        [Required]
        [StringLength(100)]
        public string repuesto { get; set; }

        [StringLength(50)]
        public string identificador { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        public int cantidad { get; set; }

        public bool activo { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_alta { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosRepuestos> MantenimientosRepuestos { get; set; }
    }
}
