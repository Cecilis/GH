namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vehiculos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehiculos()
        {
            Horas = new HashSet<Horas>();
            HorasCamioneros = new HashSet<HorasCamioneros>();
        }

        [Key]
        public int id_vehiculo { get; set; }

        [Required]
        [StringLength(10)]
        public string Patente { get; set; }

        [StringLength(50)]
        public string Marca { get; set; }

        [StringLength(50)]
        public string Modelo { get; set; }

        [StringLength(150)]
        public string Observaciones { get; set; }

        public bool Activo { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public DateTime? Fecha_Baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horas> Horas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorasCamioneros> HorasCamioneros { get; set; }
    }
}
