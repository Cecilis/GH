namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoCertificado")]
    public partial class TipoCertificado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoCertificado()
        {
            Certificados = new HashSet<Certificados>();
        }

        [Key]
        public int id_tipo_certificado { get; set; }

        [Required]
        [StringLength(50)]
        public string descripcion { get; set; }

        public bool activo { get; set; }

        public DateTime fecha_alta { get; set; }

        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Certificados> Certificados { get; set; }
    }
}
