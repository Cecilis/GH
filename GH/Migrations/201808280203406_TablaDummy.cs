namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaDummy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropIndex("dbo.Bases", "IX_Unique_BaseNombre");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Unidades", "IX_Unique_UnidadesTipoUnidadesubTipoUnidad");
            CreateTable(
                "dbo.Dummies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.name, unique: true, name: "IX_Unique_DummyNombre");
            
            AddColumn("dbo.Empleados", "Bases_id", c => c.Int());
            CreateIndex("dbo.Empleados", "Bases_id");
            AddForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies", "id");
            DropTable("dbo.Bases");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.id_base);
            
            DropForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies");
            DropIndex("dbo.Empleados", new[] { "Bases_id" });
            DropIndex("dbo.Dummies", "IX_Unique_DummyNombre");
            DropColumn("dbo.Empleados", "Bases_id");
            DropTable("dbo.Dummies");
            CreateIndex("dbo.Unidades", new[] { "tipo_unidad", "subtipo_unidad" }, unique: true, name: "IX_Unique_UnidadesTipoUnidadesubTipoUnidad");
            CreateIndex("dbo.Empleados", "id_base");
            CreateIndex("dbo.Bases", "nombre", unique: true, name: "IX_Unique_BaseNombre");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
        }
    }
}
