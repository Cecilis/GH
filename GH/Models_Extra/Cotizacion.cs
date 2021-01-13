namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cotizacion")]
    public partial class Cotizacion
    {
        public Cotizacion()
        {
            CotizacionDetalles = new HashSet<CotizacionDetalles>();
        }

        [Key]
        public int id_cotizacion { get; set; }

        public int id_usuario_alta { get; set; }

        public int id_cliente { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de alta")]
        public DateTime fecha_alta { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Observación")]
        public string observacion { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Estatus")]
        public string estatus { get; set; }

        [Display(Name = "Año")]
        public int anio { get; set; }

        [Display(Name = "Nro Cotización")]
        public long nro_cotizacion { get; set; }

        [Display(Name = "Revisión")]
        public short revision { get; set; }

        [Display(Name = "Duración Oferta")]
        public int duracion_oferta { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Vigencia")]
        public DateTime vigencia { get; set; }

        public int condicion_pago { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Nombre cliente")]
        public int cliente_nombre { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Locación")]
        public string locacion { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Contacto")]
        public string contacto { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        [Required]
        [StringLength(125)]
        [Display(Name = "Tipo de Servicio")]
        public string tipo_servicio_descripcion { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        [Display(Name = "Descripción del servicio")]
        public string servicio_descripcion { get; set; }

        [Display(Name = "Estatus aprobación")]
        public bool cotizacion_aceptada { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Decisión")]
        public DateTime fecha_decision { get; set; }

        public virtual Clientes Clientes { get; set; }
        
        public virtual Contrato Contrato { get; set; }

        public virtual ICollection<CotizacionDetalles> CotizacionDetalles { get; set; }

    }
}
