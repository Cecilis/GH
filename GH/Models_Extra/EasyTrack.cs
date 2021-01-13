namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EasyTrack")]
    public partial class EasyTrack
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string patente { get; set; }

        public int kilometros { get; set; }

        [StringLength(50)]
        public string horas_uso { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string fecha { get; set; }

        public DateTime? fecha_update { get; set; }
    }
}
