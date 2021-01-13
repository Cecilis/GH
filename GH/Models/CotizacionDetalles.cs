using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GH.Models
{
    public partial class CotizacionDetalles
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cotizacion_detalle { get; set; }

        [Required]
        [Display(Name = "ID cotización")]
        [Index("IX_Unique_CotizacionDetallesItemUM", 1, IsUnique = true)]
        public int id_cotizacion { get; set; }

        [Required]
        [Display(Name = "ID cotización")]
        [Index("IX_Unique_CotizacionDetallesItemUM", 2, IsUnique = true)]
        public int id_item { get; set; }

        [Required]
        [Display(Name = "ID unidad medida")]
        [Index("IX_Unique_CotizacionDetallesItemUM", 3, IsUnique = true)]
        public int? id_unidad_medida { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        [Display(Name = "Precio sugerido")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? precio_item_servicio { get; set; } = 0;

        [Display(Name = "Impuesto % sugerido")]
        [DisplayFormat(DataFormatString = @"{0:#\%}")]
        public Nullable<decimal> impuesto_item_servicio { get; set; } = 0;

        [Required]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Index("IX_Unique_CotizacionDetallesItemUM", 4, IsUnique = true)]
        public decimal? precio { get; set; }

        [Display(Name = "Impuesto")]
        [DisplayFormat(DataFormatString = @"{0:#\%}")]
        [Index("IX_Unique_CotizacionDetallesItemUM", 5, IsUnique = true)]
        public Nullable<decimal> impuesto { get; set; } = 0;

        [Required]
        [Display(Name = "Total impuesto")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal total_impuesto { get; set; } = 0;

        [Required]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal subtotal { get; set; } 

        [Required]
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? total { get; set; }

        [Required]
        [Display(Name = "Fecha alta")]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual ItemsServicio ItemsServicio { get; set; }

        public virtual UnidadMedida UnidadMedida { get; set; }
        
    }
}
