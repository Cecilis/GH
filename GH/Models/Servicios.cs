namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Servicios
    {

        public Servicios()
        {
            TiposServicio = new HashSet<TipoServicio>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_servicio { get; set; }       

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Index("IX_Unique_ServiciosNombre", IsUnique = true)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Observación")]
        [StringLength(150)]
        public string observaciones { get; set; } = "";

        [Display(Name = "¿Activo?")]
        public bool activo { get; set; } = true;

        [Required] //No requerido en original
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")] //No requerido en original
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;

        public virtual ICollection<TipoServicio> TiposServicio { get; set; }

    }
}
