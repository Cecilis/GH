namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lugares
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lugares()
        {
            Horas = new HashSet<Horas>();
            Horas1 = new HashSet<Horas>();
            Horas2 = new HashSet<Horas>();
            HorasCamioneros = new HashSet<HorasCamioneros>();
        }

        [Key]
        public int id_lugar { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(150)]
        public string observaciones { get; set; }

        public bool activo { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        [StringLength(20)]
        public string accion { get; set; }

        public DateTime? fecha_accion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horas> Horas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horas> Horas1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horas> Horas2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorasCamioneros> HorasCamioneros { get; set; }
    }
}
