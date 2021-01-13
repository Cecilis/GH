namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NotasEstados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NotasEstados()
        {
            Notas = new HashSet<Notas>();
        }

        [Key]
        public int id_nota_estado { get; set; }

        public int id_nota { get; set; }

        public int id_estado { get; set; }

        [Required]
        [StringLength(100)]
        public string usuario { get; set; }

        [StringLength(250)]
        public string observaciones { get; set; }

        public DateTime fecha_estado { get; set; }

        public virtual Estados Estados { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notas> Notas { get; set; }
    }
}
