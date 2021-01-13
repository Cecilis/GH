namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCotizacionDetalleXServicioItems : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.CotizacionDetalles", "id_item", "dbo.ServicioItems", "id_item", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CotizacionDetalles", "id_item", "dbo.ServicioItems");
        }
    }
}
