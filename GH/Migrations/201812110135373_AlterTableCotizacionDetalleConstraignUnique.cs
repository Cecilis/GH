namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCotizacionDetalleConstraignUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItemUM");
            CreateIndex("dbo.CotizacionDetalles", new[] { "id_cotizacion", "id_item", "id_unidad_medida", "precio", "impuesto" }, unique: true, name: "IX_Unique_CotizacionDetallesItemUM");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItemUM");
            CreateIndex("dbo.CotizacionDetalles", new[] { "id_cotizacion", "id_item", "id_unidad_medida" }, unique: true, name: "IX_Unique_CotizacionDetallesItemUM");
        }
    }
}
