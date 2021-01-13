namespace GH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnidadMedida")]
    public partial class UnidadMedida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int id_unidad_medida { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        [Index("IX_Unique_UnidadMedidaDescripcion", IsUnique = true)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "Abreviatura")]
        [Index("IX_Unique_UnidadMedidaAbreviatura", IsUnique = true)]
        public string abreviatura { get; set; }

        [Required]
        [Display(Name = "¿Activa?")]
        public bool activo { get; set; } = true;

    }
}