namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AlertasMantenimientos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_alerta_mantenimiento { get; set; }

        public int id_unidad { get; set; }

        public int id_tarea { get; set; }

        public int id_medida { get; set; }

        [Required]
        [StringLength(10)]
        public string ultimo_valor { get; set; }

        [Required]
        [StringLength(10)]
        public string valor_actual { get; set; }

        public int alerta { get; set; }

        public int id_estado { get; set; }

        public int semaforo { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_alta { get; set; }

        public virtual Estados Estados { get; set; }

        public virtual Tareas Tareas { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
