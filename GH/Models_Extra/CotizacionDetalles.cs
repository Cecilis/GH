namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CotizacionDetalles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cotizacion { get; set; }

        [Key, ForeignKey("CotizacionDetalles")]
        public int id_item { get; set; }
        
        [Required]
        [StringLength(500, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        public int id_unidad_medida { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? precio { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual UnidadMedida UnidadMedida { get; set; }

        public virtual ServicioItems ServicioItems { get; set; }
    }
}
