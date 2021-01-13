namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bases
    {
        public Bases()
        {
            Empleados = new HashSet<Empleados>();
            Personas = new HashSet<Personas>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_base { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50, MinimumLength = 2)]
        [Index("IX_Unique_BaseNombre", IsUnique = true)]
        public string nombre { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Observación")]
        [StringLength(100)]
        public string observaciones { get; set; } = "";

        [Required]
        [Display(Name = "Activo")]
        public bool activo { get; set; } = true;

        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? fecha_alta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;

        public virtual ICollection<Empleados> Empleados { get; set; }
        public virtual ICollection<Personas> Personas { get; set; }

    }
}
