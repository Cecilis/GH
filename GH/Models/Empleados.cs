namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Empleados
    {
        public Empleados()
        {
            Personas = new HashSet<Personas>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_empleado { get; set; }

        [Display(Name = "ID Base")]
        public int? id_base { get; set; } = null;

        [Required]//No Requerido en original 
        [Display(Name = "Legajo")]
        [Index("IX_Unique_Legajo", IsUnique = true)]
        public int? Legajo { get; set; }

        [Required] //No Requerido en original 
        [Display(Name = "DNI")]
        [Index("IX_Unique_EmpleadosDNI", IsUnique = true)]
        public int? DNI { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        [StringLength(50, MinimumLength = 2)]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(50, MinimumLength = 2)]
        [Index("IX_Unique_EmpleadosNombre", IsUnique = true)]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Mail")]
        [StringLength(50, MinimumLength = 2)]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "Tipo empleado")]
        [ForeignKey("Gremios")]
        public int? Tipo_Empleado { get; set; } = null;

        [Display(Name = "Categoría")]
        public int? id_categoria { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? FechaBaja { get; set; } = null;

        [ForeignKey("id_categoria")]
        public virtual Categorias Categorias { get; set; }

        public virtual Bases Bases { get; set; }

        public virtual Gremios Gremios { get; set; }

        public virtual ICollection<Personas> Personas { get; set; }
    }
}
