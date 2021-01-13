namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Certificados
    {
        [Key]
        public int id_certificado { get; set; }

        public int id_unidad { get; set; }

        public int id_tipo_certificado { get; set; }

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

        public virtual TipoCertificado TipoCertificado { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
