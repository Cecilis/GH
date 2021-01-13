namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categorias
    {

        public Categorias()
        {
            Empleados = new HashSet<Empleados>();
            Personas = new HashSet<Personas>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_categoria { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        [StringLength(100, MinimumLength = 2)]
        public string descripcion { get; set; }

        [Required] //No existe en el original
        [Display(Name = "Activo")]
        public bool activo { get; set; } = true;

        public virtual ICollection<Empleados> Empleados { get; set; }
        public virtual ICollection<Personas> Personas { get; set; }
    }
}
