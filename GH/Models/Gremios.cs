namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Gremios
    {
        Gremios(){
            Empleados = new HashSet<Empleados>();
            Personas = new HashSet<Personas>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_gremio { get; set; }

        [Required]
        [Display(Name = "Tipo empleado")]
        [StringLength(50)]
        public string gremio { get; set; }

        [Display(Name = "Activo")]
        public bool activo { get; set; } = true;

        public virtual ICollection<Empleados> Empleados { get; set; }
        public virtual ICollection<Personas> Personas { get; set; }

    }
}
