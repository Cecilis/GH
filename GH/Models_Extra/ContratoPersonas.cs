namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContratoPersonas
    {
        [Key]
        public int id_contrato_persona { get; set; }

        public int id_contrato { get; set; }

        public int id_persona { get; set; }

        public virtual Contrato Contrato { get; set; }
    }
}
