namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustestatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categorias", "activo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categorias", "activo");
        }
    }
}
