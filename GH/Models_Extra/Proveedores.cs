namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Proveedores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proveedores()
        {
            Presupuestos = new HashSet<Presupuestos>();
        }

        [Key]
        public int id_proveedor { get; set; }

        [StringLength(15)]
        public string cuit { get; set; }

        [StringLength(150)]
        public string razon_social { get; set; }

        [StringLength(150)]
        public string nombre_comercial { get; set; }

        [StringLength(150)]
        public string domicilio { get; set; }

        public int? cod_provincia { get; set; }

        [StringLength(100)]
        public string localidad { get; set; }

        public int? cod_postal { get; set; }

        [StringLength(50)]
        public string telefono { get; set; }

        [StringLength(100)]
        public string contacto { get; set; }

        [StringLength(100)]
        public string mail { get; set; }

        [StringLength(150)]
        public string web { get; set; }

        public DateTime fecha_alta { get; set; }

        public bool activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
    }
}
