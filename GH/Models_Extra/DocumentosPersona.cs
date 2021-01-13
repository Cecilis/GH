namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocumentosPersona")]
    public partial class DocumentosPersona
    {
        [Key]
        public int id_documento_Persona { get; set; }

        public int id_tipo_documento { get; set; }

        public int id_persona { get; set; }

        [Required]
        [StringLength(500)]
        public string ruta { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_documento { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_alta { get; set; }

        public int id_usuario { get; set; }

        public bool activo { get; set; }

        public virtual Personas Personas { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }
    }
}
