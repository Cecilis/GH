namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCotizacionColumnObservacion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cotizacion", "fecha_evaluacion", c => c.DateTime());
            AlterColumn("dbo.Cotizacion", "fecha_aprobacion", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cotizacion", "fecha_aprobacion", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Cotizacion", "fecha_evaluacion", c => c.DateTime(nullable: false));
        }
    }
}
