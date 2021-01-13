using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GH.ViewModels
{
    public class CotizacionAprobacionModel
    {
        [Required]
        [Display(Name = "ID Cotización")]
        public int id_cotizacion { get; set; }

        [Required]
        [Display(Name = "Estatus")]
        public string estatus { get; set; } = "";

        /*Aprobación*/
        [Required]
        [Display(Name = "Estatus aprobación")]
        public bool estatus_aprobacion { get; set; } = false;

        [NotMapped]
        [Display(Name = "Estatus evaluación descripcion")]
        public string estatus_evaluacion_descripcion { get; set; } = "";

        [StringLength(500)]
        [Display(Name = "Observación")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string observacion_aprobacion { get; set; } = "";

        //[Required(AllowEmptyStrings = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de aprobación")]
        public DateTime? fecha_aprobacion { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "ID Usuario")]
        public int? id_usuario_aprobacion { get; set; } = 0;
    }
}


