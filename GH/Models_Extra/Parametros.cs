namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parametros
    {
        [Key]
        public int id_parametro { get; set; }

        [Required]
        [StringLength(50)]
        public string parametro { get; set; }

        [StringLength(250)]
        public string descripcion { get; set; }

        public int valor { get; set; }
    }
}
