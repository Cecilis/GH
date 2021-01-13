namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteItemServiciosCotizacionCotiacionDetallesII : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ServicioItems", new[] { "id_tipo_servicio" });
            DropIndex("dbo.ServicioItems", new[] { "id_unidad_medida" });
            DropIndex("dbo.CotizacionDetalles", new[] { "id_cotizacion" });
            DropIndex("dbo.CotizacionDetalles", new[] { "id_item" });
            DropIndex("dbo.CotizacionDetalles", new[] { "id_unidad_medida" });
            CreateIndex("dbo.ServicioItems", new[] { "id_tipo_servicio", "id_unidad_medida" }, unique: true, name: "IX_Unique_ServicioItemsTipoServUMedida");
            CreateIndex("dbo.CotizacionDetalles", "id_cotizacion", unique: true, name: "IX_Unique_CotizacionDetallesCotizacion");
            CreateIndex("dbo.CotizacionDetalles", new[] { "id_item", "id_unidad_medida" }, unique: true, name: "IX_Unique_CotizacionDetallesItem");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItem");
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesCotizacion");
            DropIndex("dbo.ServicioItems", "IX_Unique_ServicioItemsTipoServUMedida");
            CreateIndex("dbo.CotizacionDetalles", "id_unidad_medida");
            CreateIndex("dbo.CotizacionDetalles", "id_item");
            CreateIndex("dbo.CotizacionDetalles", "id_cotizacion");
            CreateIndex("dbo.ServicioItems", "id_unidad_medida");
            CreateIndex("dbo.ServicioItems", "id_tipo_servicio");
        }
    }
}
