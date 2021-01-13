namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OCDetalles
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_orden { get; set; }

        [Key]
        [Column(Order = 1)]
        public int id_detalle { get; set; }

        [Required]
        [StringLength(50)]
        public string descripcion { get; set; }

        public int cantidad { get; set; }

        public decimal precio_unitario { get; set; }
    }
}
