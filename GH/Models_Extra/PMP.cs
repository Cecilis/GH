namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMP")]
    public partial class PMP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PMP()
        {
            PMPDetalle = new HashSet<PMPDetalle>();
        }

        [Key]
        public int id_pmp { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        public int id_tipo_unidad { get; set; }

        public int? id_subtipo_unidad { get; set; }

        [StringLength(150)]
        public string observaciones { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMPDetalle> PMPDetalle { get; set; }
    }
}
