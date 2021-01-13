namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubTipoUnidad")]
    public partial class SubTipoUnidad
    {
        public SubTipoUnidad()
        {
            Unidades = new HashSet<Unidades>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_subtipo { get; set; }

        [Required]
        [Index("IX_Unique_SubTipoUnidadesTipoUnidadDescripcion", 1, IsUnique = true)]
        [Display(Name = "Tipo Unidad")]
        public int id_tipo { get; set; }

        [Required]
        [Index("IX_Unique_SubTipoUnidadesTipoUnidadDescripcion", 2, IsUnique = true)]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool activo { get; set; } = true;

        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;

        public virtual TipoUnidad TipoUnidad { get; set; }

        public virtual ICollection<Unidades> Unidades { get; set; }
    }
}
