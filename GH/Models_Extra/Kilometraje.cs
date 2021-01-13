namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kilometraje")]
    public partial class Kilometraje
    {
        [Key]
        public int id_kilometraje { get; set; }

        public int id_unidad { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public int kilometros { get; set; }

        [StringLength(50)]
        public string usuario_carga { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
