namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    //TODO: Enlazar con los usuarios de la aplicaci�n
    [Table("Cotizacion")]
    public partial class Cotizacion
    {

        private enum Estatus
        {
            EnEdicion = '0',
            Creada = '1',
            EnEvaluacion = '2',
            PendientePorAprobacion = '3',
            ContratoGenerado = '4'
        }


        public Cotizacion()
        {
            CotizacionDetalles = new HashSet<CotizacionDetalles>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cotizacion { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Estatus")]
        public string estatus { get; set; } = "0";

        [Required]
        [Display(Name = "A�o")]
        public int anio { get; set; }

        [Required]
        [Display(Name = "Nro cotizaci�n")]
        public long nro_cotizacion { get; set; } = 0;

        [Required]
        [Display(Name = "Revisi�n")]
        public short revision { get; set; } = 1;

        [Required]
        [Display(Name = "Dureci�n oferta")]
        public int duracion_oferta { get; set; } = 0;

        //Fecha desde la que inicia la oferta
        [Display(Name = "Vigencia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime vigencia { get; set; }

        [Required]
        [Display(Name = "Condici�n pago")]
        public int condicion_pago { get; set; }

        /*Cliente Info*/
        [Required]
        [Display(Name = "ID Cliente")]
        public int id_cliente { get; set; }

        [Required]
        [Display(Name = "Cliente denominaci�n")]
        [StringLength(100, MinimumLength = 2)]
        public string cliente_denominacion { get; set; }

        [Required]
        [Display(Name = "Cliente CUIT")]
        public long? cliente_CUIT { get; set; }

        [Required]
        [Display(Name = "Cliente Localidad")]
        public int cliente_id_localidad { get; set; } = 0;

        [Display(Name = "Cliente Localidad")]
        public string cliente_localidad { get; set; }

        [Required]
        [Display(Name = "Cliente direcci�n")]
        [StringLength(100, MinimumLength = 2)]
        public string cliente_direccion { get; set; }

        [Required]
        [Display(Name = "Cliente tel�fono")]
        [StringLength(100, MinimumLength = 2)]
        public string cliente_telefono { get; set; }

        [Required]
        [Display(Name = "Cliente mail")]
        [StringLength(100, MinimumLength = 2)]
        public string cliente_mail { get; set; }

        /*Servicio Info*/
        [Required]
        [Display(Name = "ID Localidad")]
        [ForeignKey("Localidad")]
        public int id_localidad { get; set; } = 0;

        [Required]
        [Display(Name = "Contacto")]
        [StringLength(255, MinimumLength = 2)]        
        public string contacto { get; set; }

        [Required]
        [Display(Name = "Locaci�n")]
        [StringLength(100, MinimumLength = 2)]
        public string locacion { get; set; } = "";

        [Required]        
        [Display(Name = "Tel�fono")]
        [StringLength(15, MinimumLength = 2)]
        public string telefono { get; set; }

        [Required]
        [Display(Name = "ID Tipo de servicio")]
        [ForeignKey("TipoServicio")]
        public int id_tipo_servicio { get; set; }

        [Required]
        [StringLength(125, MinimumLength = 2)]
        [Display(Name = "Tipo de Servicio")]
        public string tipo_servicio_descripcion { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1)]
        [Display(Name = "Descripci�n servicio")]
        public string servicio_descripcion { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Observaci�n")]
        [StringLength(500)]
        public string observacion_alta { get; set; } = "";

        /* Autom�ticos*/
        [Required]
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? fecha_alta { get; set; }

        [Required]
        [Display(Name = "Usuario Alta")]
        public int? id_usuario_alta { get; set; } = 0;

        /*totales (tomados de los items)*/
        [Required]
        [Display(Name = "Total impuesto")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal total_impuesto { get; set; } = 0;

        [Required]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal subtotal { get; set; } = 0;

        [Required]
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? total { get; set; } = 0;


        /*Evaluaci�n*/

        [Required]
        [Display(Name = "Estatus evaluaci�n")]
        public bool estatus_evaluacion { get; set; } = false;

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Observaci�n evaluaci�n")]
        [StringLength(500)]
        public string observacion_evaluacion { get; set; } = "";

        [Display(Name = "Fecha evaluaci�n")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_evaluacion { get; set; } = null;

        [Required]
        [Display(Name = "Usuario evaluaci�n")]
        public int? id_usuario_evaluacion { get; set; } = 0;

        [Required]
        [Display(Name = "Estatus aprobaci�n")]
        public bool? estatus_aprobacion { get; set; } = false;

        /*Aprobaci�n*/

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Observaci�n aprobaci�n")]
        [StringLength(500)]
        public string observacion_aprobacion { get; set; } = "";

        [Display(Name = "Fecha aprobaci�n")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_aprobacion { get; set; } = null;

        [Required]
        [Display(Name = "ID usuario aprobaci�n")]
        public int? id_usuario_aprobacion { get; set; } = 0;

        public virtual Localidad Localidad { get; set; }

        [ForeignKey("id_cliente")]
        public virtual Clientes Clientes { get; set; }

        public virtual TipoServicio TipoServicio { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual ICollection<CotizacionDetalles> CotizacionDetalles { get; set; }

    }
}
