namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingerror : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Personas", "Legajo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Personas", "Legajo", c => c.Int());
        }
    }
}
