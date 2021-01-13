namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueTipoDocumentoxNombreDocumento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServicioItems", "id_item", "dbo.CotizacionDetalles");
            DropIndex("dbo.ServicioItems", new[] { "id_item" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ServicioItems", "id_item");
            AddForeignKey("dbo.ServicioItems", "id_item", "dbo.CotizacionDetalles", "id_cotizacion_detalle");
        }
    }
}
