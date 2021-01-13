namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Gremios
    {
        [Key]
        public int id_gremio { get; set; }

        [Required]
        [StringLength(50)]
        public string gremio { get; set; }

        public bool activo { get; set; }
    }
}
