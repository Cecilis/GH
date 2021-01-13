namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Notas()
        {
            NotasDetalles = new HashSet<NotasDetalles>();
            Presupuestos = new HashSet<Presupuestos>();
        }

        [Key]
        public int id_nota { get; set; }

        public DateTime fecha { get; set; }

        public DateTime? fecha_limite { get; set; }

        [Required]
        [StringLength(50)]
        public string usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string descripcion { get; set; }

        public int id_tipo_pedido { get; set; }

        public int id_destino { get; set; }

        public int id_centro_costo { get; set; }

        public int? id_unidad { get; set; }

        public int id_prioridad { get; set; }

        public int id_nota_estado { get; set; }

        [StringLength(250)]
        public string observaciones { get; set; }

        public bool activo { get; set; }

        public DateTime? fecha_baja { get; set; }

        public virtual Destinos Destinos { get; set; }

        public virtual NotasEstados NotasEstados { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotasDetalles> NotasDetalles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
    }
}
