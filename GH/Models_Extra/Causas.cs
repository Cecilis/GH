namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Causas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Causas()
        {
            Horas = new HashSet<Horas>();
            HorasCamioneros = new HashSet<HorasCamioneros>();
        }

        [Key]
        public int id_causa { get; set; }

        [Required]
        [StringLength(50)]
        public string descripcion { get; set; }

        [StringLength(150)]
        public string observaciones { get; set; }

        public bool activo { get; set; }

        public bool trabaja { get; set; }

        public DateTime? fecha_alta { get; set; }

        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horas> Horas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorasCamioneros> HorasCamioneros { get; set; }
    }
}
