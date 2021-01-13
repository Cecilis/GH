namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Programas
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_unidad { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tarea { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_medida { get; set; }

        public int disparador { get; set; }

        [Required]
        [StringLength(20)]
        public string valor_inicial { get; set; }

        [StringLength(20)]
        public string valor_actual { get; set; }

        public int alerta { get; set; }

        public int semaforo { get; set; }

        public int estado { get; set; }

        public DateTime fecha_alta { get; set; }

        public bool activo { get; set; }

        public virtual Tareas Tareas { get; set; }

        public virtual Unidades Unidades { get; set; }
    }
}
