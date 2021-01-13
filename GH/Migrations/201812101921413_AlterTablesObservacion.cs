namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTablesObservacion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Servicios", "observaciones", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Unidades", "Observaciones", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Unidades", "Observaciones", c => c.String(maxLength: 150));
            AlterColumn("dbo.Servicios", "observaciones", c => c.String(maxLength: 150));
        }
    }
}
