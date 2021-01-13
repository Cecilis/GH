namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteItemServiciosCotizacionCotiacionDetalles : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesCotizacion");
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItem");
            AddColumn("dbo.Cotizacion", "total_impuesto", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Cotizacion", "subtotal", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Cotizacion", "total", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.ServicioItems", "id_unidad_medida", c => c.Int(nullable: false));
            AddColumn("dbo.ServicioItems", "impuesto_fijo", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.ServicioItems", "impuesto_porcentaje", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "precio_item_servicio", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "impuesto_fijo", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "impuesto_porcentaje", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "total_impuesto", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "subtotal", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "total", c => c.Decimal(nullable: false, storeType: "money"));
            CreateIndex("dbo.ServicioItems", "id_unidad_medida");
            CreateIndex("dbo.CotizacionDetalles", "id_cotizacion");
            CreateIndex("dbo.CotizacionDetalles", "id_item");
            AddForeignKey("dbo.ServicioItems", "id_unidad_medida", "dbo.UnidadMedida", "id_unidad_medida", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServicioItems", "id_unidad_medida", "dbo.UnidadMedida");
            DropIndex("dbo.CotizacionDetalles", new[] { "id_item" });
            DropIndex("dbo.CotizacionDetalles", new[] { "id_cotizacion" });
            DropIndex("dbo.ServicioItems", new[] { "id_unidad_medida" });
            DropColumn("dbo.CotizacionDetalles", "total");
            DropColumn("dbo.CotizacionDetalles", "subtotal");
            DropColumn("dbo.CotizacionDetalles", "total_impuesto");
            DropColumn("dbo.CotizacionDetalles", "impuesto_porcentaje");
            DropColumn("dbo.CotizacionDetalles", "impuesto_fijo");
            DropColumn("dbo.CotizacionDetalles", "precio_item_servicio");
            DropColumn("dbo.ServicioItems", "impuesto_porcentaje");
            DropColumn("dbo.ServicioItems", "impuesto_fijo");
            DropColumn("dbo.ServicioItems", "id_unidad_medida");
            DropColumn("dbo.Cotizacion", "total");
            DropColumn("dbo.Cotizacion", "subtotal");
            DropColumn("dbo.Cotizacion", "total_impuesto");
            CreateIndex("dbo.CotizacionDetalles", "id_item", unique: true, name: "IX_Unique_CotizacionDetallesItem");
            CreateIndex("dbo.CotizacionDetalles", "id_cotizacion", unique: true, name: "IX_Unique_CotizacionDetallesCotizacion");
        }
    }
}
