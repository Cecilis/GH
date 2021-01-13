namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteporGremiosLegajos : DbMigration
    {
        public override void Up()
        {

            RenameTable(name: "dbo.PersonasContrato", newName: "ContratoPersonas");
            RenameTable(name: "dbo.UnidadesDocumentacion", newName: "DocumentacionUnidades");
            DropForeignKey("dbo.Bases", "Empleados_id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.Categorias", "Empleados_id_empleado", "dbo.Empleados");
            DropIndex("dbo.Bases", new[] { "Empleados_id_empleado" });
            DropIndex("dbo.Categorias", new[] { "Empleados_id_empleado" });
            //DropColumn("dbo.Empleados", "id_base");
            //DropColumn("dbo.Empleados", "id_categoria");
            //RenameColumn(table: "dbo.Empleados", name: "Empleados_id_empleado", newName: "id_base");
            //RenameColumn(table: "dbo.Empleados", name: "Empleados_id_empleado", newName: "id_categoria");
            DropPrimaryKey("dbo.ContratoPersonas");
            DropPrimaryKey("dbo.DocumentacionUnidades");
            CreateTable(
                "dbo.Gremios",
                c => new
                    {
                        id_gremio = c.Int(nullable: false, identity: true),
                        gremio = c.String(nullable: false, maxLength: 50),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_gremio);
            
            AddColumn("dbo.Personas", "id_base", c => c.Int());
            AddPrimaryKey("dbo.ContratoPersonas", new[] { "Contrato_id_contrato", "Personas_id_persona" });
            AddPrimaryKey("dbo.DocumentacionUnidades", new[] { "Documentacion_id_documento", "Unidades_id_unidad" });
            CreateIndex("dbo.Empleados", "id_base");
            CreateIndex("dbo.Empleados", "Legajo", unique: true, name: "IX_Unique_Legajo");
            CreateIndex("dbo.Empleados", "Tipo_Empleado");
            CreateIndex("dbo.Empleados", "id_categoria");
            CreateIndex("dbo.Personas", "id_base");
            CreateIndex("dbo.Personas", "id_tipo_empleado");
            CreateIndex("dbo.Personas", "id_categoria");
            AddForeignKey("dbo.Personas", "id_base", "dbo.Bases", "id_base");
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            //AddForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios", "id_gremio", cascadeDelete: false);
            AddForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios", "id_gremio");
            AddForeignKey("dbo.Personas", "id_categoria", "dbo.Categorias", "id_categoria", cascadeDelete: false);            
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: false);
            AddForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias", "id_categoria", cascadeDelete: false);
            DropColumn("dbo.Bases", "Empleados_id_empleado");
            DropColumn("dbo.Categorias", "Empleados_id_empleado");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Categorias", "Empleados_id_empleado", c => c.Int());
            AddColumn("dbo.Bases", "Empleados_id_empleado", c => c.Int());
            DropForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias");
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            DropForeignKey("dbo.Personas", "id_categoria", "dbo.Categorias");
            DropForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios");
            DropForeignKey("dbo.Personas", "id_base", "dbo.Bases");
            DropIndex("dbo.Personas", new[] { "id_categoria" });
            DropIndex("dbo.Personas", new[] { "id_tipo_empleado" });
            DropIndex("dbo.Personas", new[] { "id_base" });
            DropIndex("dbo.Empleados", new[] { "id_categoria" });
            DropIndex("dbo.Empleados", new[] { "Tipo_Empleado" });
            DropIndex("dbo.Empleados", "IX_Unique_Legajo");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropPrimaryKey("dbo.DocumentacionUnidades");
            DropPrimaryKey("dbo.ContratoPersonas");
            DropColumn("dbo.Personas", "id_base");
            DropTable("dbo.Gremios");
            AddPrimaryKey("dbo.DocumentacionUnidades", new[] { "Unidades_id_unidad", "Documentacion_id_documento" });
            AddPrimaryKey("dbo.ContratoPersonas", new[] { "Personas_id_persona", "Contrato_id_contrato" });
            RenameColumn(table: "dbo.Empleados", name: "id_categoria", newName: "Empleados_id_empleado");
            RenameColumn(table: "dbo.Empleados", name: "id_base", newName: "Empleados_id_empleado");
            AddColumn("dbo.Empleados", "id_categoria", c => c.Int(nullable: false));
            AddColumn("dbo.Empleados", "id_base", c => c.Int(nullable: false));
            CreateIndex("dbo.Categorias", "Empleados_id_empleado");
            CreateIndex("dbo.Bases", "Empleados_id_empleado");
            AddForeignKey("dbo.Categorias", "Empleados_id_empleado", "dbo.Empleados", "id_empleado");
            AddForeignKey("dbo.Bases", "Empleados_id_empleado", "dbo.Empleados", "id_empleado");
            RenameTable(name: "dbo.DocumentacionUnidades", newName: "UnidadesDocumentacion");
            RenameTable(name: "dbo.ContratoPersonas", newName: "PersonasContrato");
        }
    }
}
