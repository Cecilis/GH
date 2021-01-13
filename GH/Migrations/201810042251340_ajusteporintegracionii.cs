namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajusteporintegracionii : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Bases", newName: "Base");
            RenameTable(name: "dbo.DocumentacionUnidades", newName: "UnidadesDocumentacion");
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropForeignKey("dbo.ContratoPersonas", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoUnidades", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoUnidades", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoUnidades", "id_unidad", "dbo.Unidades");
            DropForeignKey("dbo.ContratoPersonas", "id_persona", "dbo.Personas");
            DropForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Empleados", new[] { "id_categoria" });
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasContratos");
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasPersonas");
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesContrato");
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesUnidad");
            DropIndex("dbo.ContratoUnidades", new[] { "Personas_id_persona" });
            DropIndex("dbo.Dummies", "IX_Unique_DummyNombre");
            DropPrimaryKey("dbo.UnidadesDocumentacion");
            CreateTable(
                "dbo.EmpleadosBase",
                c => new
                    {
                        Empleados_id_empleado = c.Int(nullable: false),
                        Base_id_base = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Empleados_id_empleado, t.Base_id_base })
                .ForeignKey("dbo.Empleados", t => t.Empleados_id_empleado, cascadeDelete: true)
                .ForeignKey("dbo.Base", t => t.Base_id_base, cascadeDelete: true)
                .Index(t => t.Empleados_id_empleado)
                .Index(t => t.Base_id_base);
            
            CreateTable(
                "dbo.UnidadesContrato",
                c => new
                    {
                        Unidades_id_unidad = c.Int(nullable: false),
                        Contrato_id_contrato = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Unidades_id_unidad, t.Contrato_id_contrato })
                .ForeignKey("dbo.Unidades", t => t.Unidades_id_unidad, cascadeDelete: true)
                .ForeignKey("dbo.Contrato", t => t.Contrato_id_contrato, cascadeDelete: true)
                .Index(t => t.Unidades_id_unidad)
                .Index(t => t.Contrato_id_contrato);
            
            AddColumn("dbo.Categorias", "Empleados_id_empleado", c => c.Int());
            AddColumn("dbo.Personas", "Contrato_id_contrato", c => c.Int());
            AddColumn("dbo.Unidades", "Personas_id_persona", c => c.Int());
            AddPrimaryKey("dbo.UnidadesDocumentacion", new[] { "Unidades_id_unidad", "Documentacion_id_documento" });
            CreateIndex("dbo.Categorias", "Empleados_id_empleado");
            CreateIndex("dbo.Personas", "id_empleado");
            CreateIndex("dbo.Personas", "Contrato_id_contrato");
            CreateIndex("dbo.Unidades", "Personas_id_persona");
            AddForeignKey("dbo.Personas", "id_empleado", "dbo.Empleados", "id_empleado", cascadeDelete: true);
            AddForeignKey("dbo.Unidades", "Personas_id_persona", "dbo.Personas", "id_persona");
            AddForeignKey("dbo.Personas", "Contrato_id_contrato", "dbo.Contrato", "id_contrato");
            AddForeignKey("dbo.Categorias", "Empleados_id_empleado", "dbo.Empleados", "id_empleado");
            DropTable("dbo.ContratoPersonas");
            DropTable("dbo.ContratoUnidades");
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
            
            CreateTable(
                "dbo.ContratoUnidades",
                c => new
                    {
                        id_contrato_unidad = c.Int(nullable: false, identity: true),
                        id_contrato = c.Int(nullable: false),
                        id_unidad = c.Int(nullable: false),
                        Personas_id_persona = c.Int(),
                    })
                .PrimaryKey(t => t.id_contrato_unidad);
            
            CreateTable(
                "dbo.ContratoPersonas",
                c => new
                    {
                        id_contrato_persona = c.Int(nullable: false, identity: true),
                        id_contrato = c.Int(nullable: false),
                        id_persona = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_contrato_persona);
            
            DropForeignKey("dbo.Categorias", "Empleados_id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.Personas", "Contrato_id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.Unidades", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.Personas", "id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.UnidadesContrato", "Contrato_id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.UnidadesContrato", "Unidades_id_unidad", "dbo.Unidades");
            DropForeignKey("dbo.EmpleadosBase", "Base_id_base", "dbo.Base");
            DropForeignKey("dbo.EmpleadosBase", "Empleados_id_empleado", "dbo.Empleados");
            DropIndex("dbo.UnidadesContrato", new[] { "Contrato_id_contrato" });
            DropIndex("dbo.UnidadesContrato", new[] { "Unidades_id_unidad" });
            DropIndex("dbo.EmpleadosBase", new[] { "Base_id_base" });
            DropIndex("dbo.EmpleadosBase", new[] { "Empleados_id_empleado" });
            DropIndex("dbo.Unidades", new[] { "Personas_id_persona" });
            DropIndex("dbo.Personas", new[] { "Contrato_id_contrato" });
            DropIndex("dbo.Personas", new[] { "id_empleado" });
            DropIndex("dbo.Categorias", new[] { "Empleados_id_empleado" });
            DropPrimaryKey("dbo.UnidadesDocumentacion");
            DropColumn("dbo.Unidades", "Personas_id_persona");
            DropColumn("dbo.Personas", "Contrato_id_contrato");
            DropColumn("dbo.Categorias", "Empleados_id_empleado");
            DropTable("dbo.UnidadesContrato");
            DropTable("dbo.EmpleadosBase");
            AddPrimaryKey("dbo.UnidadesDocumentacion", new[] { "Documentacion_id_documento", "Unidades_id_unidad" });
            CreateIndex("dbo.Dummies", "name", unique: true, name: "IX_Unique_DummyNombre");
            CreateIndex("dbo.ContratoUnidades", "Personas_id_persona");
            CreateIndex("dbo.ContratoUnidades", "id_unidad", unique: true, name: "IX_Unique_ContratoUnidadesUnidad");
            CreateIndex("dbo.ContratoUnidades", "id_contrato", unique: true, name: "IX_Unique_ContratoUnidadesContrato");
            CreateIndex("dbo.ContratoPersonas", "id_persona", unique: true, name: "IX_Unique_ContratoPersonasPersonas");
            CreateIndex("dbo.ContratoPersonas", "id_contrato", unique: true, name: "IX_Unique_ContratoPersonasContratos");
            CreateIndex("dbo.Empleados", "id_categoria");
            CreateIndex("dbo.Empleados", "id_base");
            AddForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias", "id_categoria", cascadeDelete: true);
            AddForeignKey("dbo.ContratoPersonas", "id_persona", "dbo.Personas", "id_persona", cascadeDelete: true);
            AddForeignKey("dbo.ContratoUnidades", "id_unidad", "dbo.Unidades", "id_unidad", cascadeDelete: true);
            AddForeignKey("dbo.ContratoUnidades", "Personas_id_persona", "dbo.Personas", "id_persona");
            AddForeignKey("dbo.ContratoUnidades", "id_contrato", "dbo.Contrato", "id_contrato", cascadeDelete: true);
            AddForeignKey("dbo.ContratoPersonas", "id_contrato", "dbo.Contrato", "id_contrato", cascadeDelete: true);
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
            RenameTable(name: "dbo.UnidadesDocumentacion", newName: "DocumentacionUnidades");
            RenameTable(name: "dbo.Base", newName: "Bases");
        }
    }
}
