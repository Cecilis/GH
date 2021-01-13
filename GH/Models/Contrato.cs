namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contrato")]
    public partial class Contrato
    {

        public Contrato()
        {
            Personas = new HashSet<Personas>();
            Unidades = new HashSet<Unidades>();
        }

        /*Relación uno a uno con cotización siendo contrato dependiente de la cotizacion*/
        [Key, ForeignKey("Cotizacion")]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_contrato { get; set; }

        [Required]
        [Display(Name = "ID Cotización")]
        public int id_cotizacion { get; set; }

        [Required]
        [Display(Name = "CUIT")]
        [StringLength(100, MinimumLength = 2)]
        public string cuit_facturacion { get; set; }


        [Required]
        [Display(Name = "Fecha inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fecha_inicio { get; set; }

        [Display(Name = "Fecha fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_fin { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }
        
        public virtual ICollection<Personas> Personas { get; set; }
        
        public virtual ICollection<Unidades> Unidades { get; set; }
    }
}
