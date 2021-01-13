using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GH.ViewModels
{
    public class CotizacionContratoModel
    {
        [Required]
        [Display(Name = "Código")]
        public string codigo { get; set; }

        [Required]
        [Display(Name = "Revisión")]
        public string revision { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public string fecha { get; set; }

        [Required]
        [Display(Name = "Nro Cotización")]
        public string nro_cotizacion { get; set; }

        [Required]
        [Display(Name = "Contacto")]
        public string contacto { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        public string localidad { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }

        [Required]
        [Display(Name = "Tipo servicio")]
        public string tipo_servicio { get; set; }

        [Required]
        [Display(Name = "Duración semanal")]
        public string duracion_semanal { get; set; }

        [Required]
        [Display(Name = "Plazo servicio")]
        public string plazo_servicio { get; set; }

        [Required]
        [Display(Name = "Descripción de la carga")]
        public string descripcion_carga { get; set; }

        [Required]
        [Display(Name = "Volumén")]
        public string volumen { get; set; }

        [Required]
        [Display(Name = "Densidad")]
        public string densidad { get; set; }

        [Required]
        [Display(Name = "Peso")]
        public string peso { get; set; }

        [Required]
        [Display(Name = "Lugar prestación")]
        public string lugar_prestacion { get; set; }

        [Required]
        [Display(Name = "Lugar descarga")]
        public string lugar_descarga { get; set; }

        [Required]
        [Display(Name = "Distancia estimada")]
        public string distancia_estimada { get; set; }

        [Required]
        [Display(Name = "Volumén mensual_estimado")]
        public string volumen_mensual_estimado { get; set; }

        [Required]
        [Display(Name = "Tipo servicio observacion")]
        public string tipo_servicio_observacion { get; set; }

        [Required]
        [Display(Name = "Descripcion ")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Items")]
        public List<CotizacionContratoItemModel> items { get; set; }

        [Required]
        [Display(Name = "Nota Items")]
        public string nota_items { get; set; }

        [Required]
        [Display(Name = "Duracion oferta")]
        public string duracion_oferta { get; set; }

        [Required]
        [Display(Name = "Condición pago")]
        public string condicion_pago { get; set; }

        [Required]
        [Display(Name = "Plazo entrega")]
        public string plazo_entrega { get; set; }

        [Required]
        [Display(Name = "Cotización observacion")]
        public string cotizacion_observacion { get; set; }

        [Required]
        [Display(Name = "Nota")]
        public string nota { get; set; }

        [Required]
        [Display(Name = "Decisión")]
        public bool cotizacion_decision { get; set; } = false;

        [Required]
        [Display(Name = "Nombre")]
        public string nombre_firma { get; set; }

        [Required]
        [Display(Name = "Cargo")]
        public string cargo_firma { get; set; }

        [Required]
        [Display(Name = "Compañia")]
        public string compañia_firma { get; set; }

    }
}


