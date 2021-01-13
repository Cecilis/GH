namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaDummyFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bases",
                c => new
                    {
                        id_base = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 255),
                        observaciones = c.String(nullable: false, maxLength: 100),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_base)
                .Index(t => t.nombre, unique: true, name: "IX_Unique_BaseNombre");
            
            CreateIndex("dbo.Empleados", "id_base");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Bases", "IX_Unique_BaseNombre");
            DropTable("dbo.Bases");
        }
    }
}
