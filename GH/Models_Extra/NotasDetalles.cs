namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NotasDetalles
    {
        [Key]
        [Column(Order = 0)]
        public int id_detalle { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_nota { get; set; }

        public int cantidad { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string proveedor_sugerido { get; set; }

        public virtual Notas Notas { get; set; }
    }
}
