namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBaseObservacion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bases", "observaciones", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bases", "observaciones", c => c.String(maxLength: 100));
        }
    }
}
