namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ItemsServicio
    {

        public ItemsServicio()
        {
            CotizacionDetalles = new HashSet<CotizacionDetalles>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_item { get; set; }

        [Required]
        [Display(Name = "Tipo Servicio")]
        [Index("IX_Unique_ItemsServicioTipoUMDescripcion", 1, IsUnique = true)]
        public int id_tipo_servicio { get; set; }

        [Required]
        [Display(Name = "ID unidad medida")]
        [ForeignKey("UnidadMedida")]
        [Index("IX_Unique_ItemsServicioTipoUMDescripcion", 2, IsUnique = true)]
        public int id_unidad_medida { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [StringLength(255, MinimumLength = 2)]
        [Index("IX_Unique_ItemsServicioTipoUMDescripcion", 3, IsUnique = true)]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal precio { get; set; } 

        [Display(Name = "Impuesto %")]
        [DisplayFormat(DataFormatString = @"{0:#\%}")]
        public Nullable<decimal> impuesto { get; set; } = 0;

        [Required]
        [Display(Name = "¿Activo?")]
        public bool activo { get; set; } = true;

        public virtual TipoServicio TipoServicio { get; set; }

        public virtual UnidadMedida UnidadMedida { get; set; }

        public virtual ICollection<CotizacionDetalles> CotizacionDetalles { get; set; }

    }
}
