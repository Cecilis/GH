namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contrato")]
    public partial class Contrato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contrato()
        {
            ContratoPersonas = new HashSet<ContratoPersonas>();
            ContratoUnidades = new HashSet<ContratoUnidades>();
        }
        /*Relación uno a uno con cotización siendo contrato dependiente de la cotizacion*/
        [Key, ForeignKey("Cotizacion")]
        public int id_contrato { get; set; }

        [Required]
        public int id_cotizacion { get; set; }

        [Required]
        [StringLength(20)]
        public string cuit_facturacion { get; set; }

        [Required]
        public DateTime fecha_inicio { get; set; }

        [Required]
        public DateTime fecha_fin { get; set; }
        
        public virtual Cotizacion Cotizacion { get; set; }
        
        public virtual ICollection<ContratoPersonas> ContratoPersonas { get; set; }
        
        public virtual ICollection<ContratoUnidades> ContratoUnidades { get; set; }
    }
}
