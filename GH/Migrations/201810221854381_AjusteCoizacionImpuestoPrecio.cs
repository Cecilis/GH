namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteCoizacionImpuestoPrecio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioItems", "impuesto", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CotizacionDetalles", "impuesto_item_servicio", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CotizacionDetalles", "impuesto", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.ServicioItems", "impuesto_fijo");
            DropColumn("dbo.ServicioItems", "impuesto_porcentaje");
            DropColumn("dbo.CotizacionDetalles", "impuesto_fijo");
            DropColumn("dbo.CotizacionDetalles", "impuesto_porcentaje");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CotizacionDetalles", "impuesto_porcentaje", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.CotizacionDetalles", "impuesto_fijo", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.ServicioItems", "impuesto_porcentaje", c => c.Decimal(storeType: "money"));
            AddColumn("dbo.ServicioItems", "impuesto_fijo", c => c.Decimal(storeType: "money"));
            DropColumn("dbo.CotizacionDetalles", "impuesto");
            DropColumn("dbo.CotizacionDetalles", "impuesto_item_servicio");
            DropColumn("dbo.ServicioItems", "impuesto");
        }
    }
}
