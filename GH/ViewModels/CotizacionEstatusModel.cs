using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GH.ViewModels
{
    public class CotizacionEstatusModel
    {
        [Required]
        [Display(Name = "ID Cotización")]
        public int id_cotizacion { get; set; }

        [Required]
        [Display(Name = "Estatus")]
        public string estatus { get; set; } = "0";

        /*Aprobación*/
        [Required]
        [Display(Name = "Estatus aprobación")]
        public bool estatus_aprobacion { get; set; } = false;
    }
}


