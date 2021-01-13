namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clientes
    {
        public Clientes()
        {
            Cotizacion = new HashSet<Cotizacion>();
            //Servicios = new HashSet<Servicios>();
        }

        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cliente { get; set; }

        [Required]
        [Display(Name = "Denominación")]
        [Index("IX_Unique_ClientesDenominacion", 1, IsUnique = true)]
        [StringLength(100, MinimumLength = 2)]
        public string Denominacion { get; set; }

        [Required] //No requerido en original
        [Display(Name = "CUIT")]
        public long? CUIT { get; set; } = 0;

        [Required] //No requerido en original
        [Display(Name = "Dirección")]
        [StringLength(100, MinimumLength = 2)]
        public string Direccion { get; set; } = "";

        [Required]//No requerido en original
        [Display(Name = "Teléfono")]
        [StringLength(100)]
        public string Telefono { get; set; } = "";


        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "E-Mail")]
        public string Mail { get; set; } = "";

        [Required] //No requerido en original
        [Display(Name = "ID Localidad")]
        public int id_localidad { get; set; } = 1;

        /**Dejado por compatibilidad con la tabla original, evaluar su eliminación ya que es redundante*/
        [NotMapped] //No requerido en original
        [Display(Name = "Localidad")]
        [StringLength(100)]
        public string Localidad { get; set; } = "";

        [Required] //No requerido en original
        [Display(Name = "Activo")]
        public bool? activo { get; set; } = true;

        [Required] //No requerido en original
        [Display(Name = "Fecha alta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime fecha_alta { get; set; } = DateTime.Now;

        [Display(Name = "Fecha baja")] //No requerido en original
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_baja { get; set; } = null;

        public virtual Localidad Localidades { get; set; }

        public virtual ICollection<Cotizacion> Cotizacion { get; set; }

        //public virtual ICollection<Servicios> Servicios { get; set; }

    }
}
