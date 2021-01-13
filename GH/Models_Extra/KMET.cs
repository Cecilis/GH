namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KMET")]
    public partial class KMET
    {
        [StringLength(50)]
        public string Dominio { get; set; }

        [Key]
        [StringLength(50)]
        public string Kilometros { get; set; }
    }
}
