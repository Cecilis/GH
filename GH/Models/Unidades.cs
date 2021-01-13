namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Unidades
    {


        public Unidades()
        {
            Contratos = new HashSet<Contrato>();
            Documentacion = new HashSet<Documentacion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_unidad { get; set; }
        
        [Required]
        [Display(Name = "Tipo unidad")]
        public int tipo_unidad { get; set; }

        [Required] //No requerido en original
        [ForeignKey("SubTipoUnidad")]
        [Display(Name = "Subtipo unidad")]
        public int? subtipo_unidad { get; set; } = null;

        [Required]
        [Display(Name = "Año")]
        public int anio { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 2)]
        [Display(Name = "Patente")]
        public string Patente { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Marca")]
        public string Marca { get; set; } = "";

        [Display(Name = "Modelo")]
        [StringLength(50, MinimumLength = 2)]
        public string Modelo { get; set; } = "";

        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Chasis")]
        public string Chasis { get; set; } = "";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha compra")]
        public DateTime? Fecha_Compra { get; set; }

        [Display(Name = "ID base")]
        public int? id_base { get; set; } = null;

        [Display(Name = "Último Servicio")]
        public int? ultimo_service { get; set; } = null;

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Observación aprobación")]
        [StringLength(150)]
        public string Observaciones { get; set; } = "";

        [Required]
        [Display(Name = "¿Activo?")]
        public bool Activo { get; set; } = true;

        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;


        public virtual SubTipoUnidad SubTipoUnidad { get; set; }

        public virtual ICollection<Contrato> Contratos { get; set; }

        public virtual ICollection<Documentacion> Documentacion { get; set; }

        
    }
}
