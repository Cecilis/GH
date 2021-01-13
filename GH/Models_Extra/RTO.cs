namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RTO")]
    public partial class RTO
    {
        [Key]
        public int id_rto { get; set; }

        public int id_unidad { get; set; }

        [Required]
        [StringLength(100)]
        public string planta { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_emision { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_vencimiento { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_alta { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [Required]
        [StringLength(50)]
        public string usuario { get; set; }
    }
}
