using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GH.ViewModels
{

    //EnEdicion = '0',
    //Creada = '1',
    //EnEvaluacion = '2',
    //PendientePorAprobacion = '3',
    //ContratoGenerado = '4'

    public class CotizacionEvaluacionModel
    {
        [Required]
        [Display(Name = "ID Cotización")]
        public int id_cotizacion { get; set; }

        [Required]
        [Display(Name = "Estatus")]
        public string estatus { get; set; } = "";

        /*Evaluación*/
        [Required]
        [Display(Name = "Estatus evaluación")]
        public bool estatus_evaluacion { get; set; } = false;

        [NotMapped]
        [Display(Name = "Estatus evaluación descripcion")]
        public string estatus_evaluacion_descripcion { get; set; } = "";

        [StringLength(500)]
        [Display(Name = "Observación")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string observacion_evaluacion { get; set; } = "";

        //[Required(AllowEmptyStrings = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de evaluación")]
        public DateTime? fecha_evaluacion { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "ID Usuario")]
        public int? id_usuario_evaluacion { get; set; } = 0;


    }
}


