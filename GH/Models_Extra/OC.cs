namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OC")]
    public partial class OC
    {
        [Key]
        [Column(Order = 0)]
        public int id_orden { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_nota { get; set; }

        public int id_proveedor { get; set; }

        public int? numero { get; set; }

        public DateTime? fecha { get; set; }

        [StringLength(50)]
        public string usuario_aprobador { get; set; }

        [StringLength(50)]
        public string usuario_confecciona { get; set; }

        public int? id_envio { get; set; }

        public int? plazo_pago { get; set; }

        public int? tipo_pago { get; set; }

        [StringLength(250)]
        public string observaciones { get; set; }

        public bool emitida { get; set; }
    }
}
