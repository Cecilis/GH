namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tareas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tareas()
        {
            AlertasMantenimientos = new HashSet<AlertasMantenimientos>();
            MantenimientosProgramados = new HashSet<MantenimientosProgramados>();
            MantenimientosTareas = new HashSet<MantenimientosTareas>();
            Programas = new HashSet<Programas>();
        }

        [Key]
        public int id_tarea { get; set; }

        [Required]
        [StringLength(100)]
        public string tarea { get; set; }

        public int? id_tipo_tarea { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        public bool activo { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha_alta { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_baja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertasMantenimientos> AlertasMantenimientos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosProgramados> MantenimientosProgramados { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MantenimientosTareas> MantenimientosTareas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programas> Programas { get; set; }

        public virtual TipoTarea TipoTarea { get; set; }
    }
}
