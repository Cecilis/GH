namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoDocumento")]
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Documentacion = new HashSet<Documentacion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_tipo_documento { get; set; }

        [Required]
        [Index("IX_Unique_TipoDocumentoNombre", 1, IsUnique = true)]
        [StringLength(255, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Column(TypeName = "char")]
        [StringLength(1)]
        [Display(Name = "Aplica a")]
        public string aplica_a { get; set; }

        [Required]
        [Display(Name = "Vencimiento")]
        public bool vencimiento { get; set; }

        [Required]
        [Display(Name = "N° Dias para vencimiento")]
        public int nro_dias_vencimiento { get; set; }

        [Required]
        [Display(Name = "¿Activo?")]
        public bool activo { get; set; } = true;

        public virtual ICollection<Documentacion> Documentacion { get; set; }
    }
}
