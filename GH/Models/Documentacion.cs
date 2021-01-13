namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Documentacion")]
    public partial class Documentacion
    {

        public Documentacion()
        {
            this.Personas = new HashSet<Personas>();
            this.Unidades = new HashSet<Unidades>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_documento { get; set; }

        [Required]
        [Display(Name = "ID tipo documento")]
        public int id_tipo_documento { get; set; }

        [Required]
        [Display(Name = "Documento")]
        [Index("IX_Unique_DocumentoPersonaArchivo", IsUnique = true)]
        [StringLength(500, MinimumLength = 2)]
        public string documento { get; set; }

        [Required]
        [Display(Name = "Fecha alta")]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;
        
        public virtual ICollection<Personas> Personas { get; set; }

        public virtual ICollection<Unidades> Unidades { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }



    }
}
