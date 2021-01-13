namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletingdummies : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies");
            DropIndex("dbo.Empleados", new[] { "Bases_id" });
            DropIndex("dbo.Dummies", "IX_Unique_DummyNombre");
            DropColumn("dbo.Empleados", "Bases_id");
            DropTable("dbo.Dummies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Dummies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Empleados", "Bases_id", c => c.Int());
            CreateIndex("dbo.Dummies", "name", unique: true, name: "IX_Unique_DummyNombre");
            CreateIndex("dbo.Empleados", "Bases_id");
            AddForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies", "id");
        }
    }
}
