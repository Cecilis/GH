using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GH.ViewModels
{
    public class CotizacionContratoItemModel{
        [Required]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Unidad")]
        public string unidad { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public string precio { get; set; }
    }
}


