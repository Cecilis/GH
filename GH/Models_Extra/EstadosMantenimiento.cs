namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EstadosMantenimiento")]
    public partial class EstadosMantenimiento
    {
        [Key]
        public int id_estado { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }
    }
}
