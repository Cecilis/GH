namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoPersona")]
    public partial class TipoPersona
    {
        public TipoPersona()
        {
            Personas = new HashSet<Personas>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_tipo_persona { get; set; }

        [Required]
        [Index("IX_Unique_TipoPersonaDescripcion", IsUnique = true)]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool activo { get; set; } = true;
        
        public virtual ICollection<Personas> Personas { get; set; }
    }
}
