namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContratoUnidades
    {
        [Key]
        public int id_contrato_unidad { get; set; }

        public int id_contrato { get; set; }

        public int id_unidad { get; set; }

        public virtual Contrato Contrato { get; set; }

        public virtual Personas Personas { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
