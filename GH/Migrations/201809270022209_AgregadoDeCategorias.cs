namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregadoDeCategorias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        id_categoria = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id_categoria);
            
            CreateIndex("dbo.Empleados", "id_categoria");
            AddForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias", "id_categoria", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias");
            DropIndex("dbo.Empleados", new[] { "id_categoria" });
            DropTable("dbo.Categorias");
        }
    }
}
