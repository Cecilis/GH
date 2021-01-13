namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cierres
    {
        [Key]
        public int id_cierre { get; set; }

        public DateTime? fecha_cierre { get; set; }
    }
}
