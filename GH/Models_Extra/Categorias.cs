namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categorias
    {
        [Key]
        public int id_categoria { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }
    }
}
