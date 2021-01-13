namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajustesdatedefault : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cotizacion", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cotizacion", "fecha_alta", c => c.DateTime(nullable: false));
        }
    }
}
