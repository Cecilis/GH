namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MantenimientosProgramados
    {
        [Key]
        public int id_mp { get; set; }

        public int id_unidad { get; set; }

        public int id_tarea { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public int id_medida { get; set; }

        [Required]
        [StringLength(20)]
        public string valor_realizado { get; set; }

        [Required]
        [StringLength(20)]
        public string valor_planificado { get; set; }

        [Required]
        [StringLength(50)]
        public string usuario { get; set; }

        public virtual Tareas Tareas { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
