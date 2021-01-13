namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoServicio")]
    public partial class TipoServicio
    {
        public TipoServicio()
        {
            ItemsServicio = new HashSet<ItemsServicio>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_tipo_servicio { get; set; }

        [Required]
        [Display(Name = "ID Servicio")]
        [Index("IX_Unique_TipoServicioServicioNombre", 1, IsUnique = true)]
        public int id_servicio { get; set; }

        [Required]
        [Index("IX_Unique_TipoServicioServicioNombre", 2, IsUnique = true)]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "¿Activo?")]
        public bool activo { get; set; } = true;

        public virtual Servicios Servicios { get; set; }

        public virtual ICollection<ItemsServicio> ItemsServicio { get; set; }

    }
}
