namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Combustible")]
    public partial class Combustible
    {
        [Key]
        public int id_carga { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public int id_unidad { get; set; }

        public int id_chofer { get; set; }

        public int kilometraje { get; set; }

        public int id_tanque { get; set; }

        public int litros { get; set; }

        public DateTime fecha_alta { get; set; }

        [Required]
        [StringLength(100)]
        public string usuario { get; set; }

        public virtual Tanques Tanques { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
