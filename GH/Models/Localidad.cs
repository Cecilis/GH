using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GH.Models
{

    [Table("Localidad")]
    public partial class Localidad
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_localidad { get; set; }

        [Required]
        [Display(Name = "ID Provincia")]
        public int id_provincia { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        [Index("IX_Unique_Localidad", IsUnique = true)]
        [StringLength(150, MinimumLength = 2)]
        public string localidad { get; set; }

        [Required]
        [Display(Name = "¿Activo?")]
        public bool activo { get; set; } = true;

        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? fecha_alta { get; set; }

        [Display(Name = "Fecha baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;

        public virtual Provincias Provincias { get; set; }

    }
}
