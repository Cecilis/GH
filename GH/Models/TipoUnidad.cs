namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoUnidad")]
    public partial class TipoUnidad
    {
        public TipoUnidad()
        {
            SubTipoUnidad = new HashSet<SubTipoUnidad>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_tipo { get; set; }

        [Required]
        [Index("IX_Unique_TipoUnidadDescripcion", IsUnique = true)]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "¿Auto-propulsada?")]
        public bool? autopropulsada { get; set; }

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

        public virtual ICollection<SubTipoUnidad> SubTipoUnidad { get; set; }
    }
}
