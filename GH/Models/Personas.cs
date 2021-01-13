namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Personas
    {

        public Personas()
        {
            Documentacion = new HashSet<Documentacion>();
            Contratos = new HashSet<Contrato>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_persona { get; set; }

        [Display(Name = "ID Tipo Persona")]
        public int id_tipo_persona { get; set; }

        [Display(Name = "ID Base")]
        public int? id_base { get; set; }

        [Required] 
        [Display(Name = "ID Empleado")]
        public int? id_empleado { get; set; }

        //No requerido en empleado
        [Display(Name = "Legajo")]
        public int? Legajo { get; set; }

        [Required] //No requerido en empleado (original)
        [Index("IX_Unique_PersonasDNI", IsUnique = true)]
        [Display(Name = "DNI")]
        public int? DNI { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "E-mail")]
        public string Mail { get; set; }
       
        [Required]
        [Display(Name = "Tipo Empleado")]
        public int? id_tipo_empleado { get; set; }

        [Display(Name = "Categoría")]
        public int? id_categoria { get; set; }

        [Required]
        [Display(Name = "¿Activo?")]
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

        public virtual ICollection<Documentacion> Documentacion { get; set; }

        public virtual ICollection<Contrato> Contratos { get; set; }

        [ForeignKey("id_tipo_persona")]
        public virtual TipoPersona TipoPersona { get; set; }

        [ForeignKey("id_categoria")]
        public virtual Categorias Categoria { get; set; }

        [ForeignKey("id_base")]
        public virtual Bases Base { get; set; }

        [ForeignKey("id_empleado")]
        public virtual Empleados Empleado { get; set; }

        [ForeignKey("id_tipo_empleado")]
        public virtual Gremios Gremio { get; set; }


    }
}
