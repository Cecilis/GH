namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCotizacionAddLocacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "locacion", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cotizacion", "locacion");
        }
    }
}
