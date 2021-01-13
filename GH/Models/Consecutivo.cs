using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GH.Models
{
    public class Consecutivo
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_consecutivo { get; set; }

        [Display(Name = "Consecutivo")]
        public long consecutivo { get; set; }

        public int anio { get; set; }
    }
}