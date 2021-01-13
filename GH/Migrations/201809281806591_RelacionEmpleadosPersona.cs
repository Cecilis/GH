namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelacionEmpleadosPersona : DbMigration
    {
        public override void Up()
        {

            //RenameTable(name: "dbo.ContratoPersonases", newName: "ContratoPersonas");
            ////RenameTable(name: "dbo.UnidadesContratoes", newName: "UnidadesContrato");
            //RenameTable(name: "dbo.DocumentacionUnidadess", newName: "DocumentacionUnidades");
            DropForeignKey("dbo.Empleados", "id_empleado", "dbo.Personas");
            DropIndex("dbo.Empleados", new[] { "id_empleado" });
            DropPrimaryKey("dbo.ContratoPersonas");
            DropPrimaryKey("dbo.DocumentacionUnidades");
            AddPrimaryKey("dbo.ContratoPersonas", new[] { "Personas_id_persona", "Contrato_id_contrato" });
            AddPrimaryKey("dbo.DocumentacionUnidades", new[] { "Unidades_id_unidad", "Documentacion_id_documento" });
            CreateIndex("dbo.Personas", "id_empleado");
            AddForeignKey("dbo.Personas", "id_empleado", "dbo.Empleados", "id_empleado", cascadeDelete: true);
            //AlterColumn("dbo.Personas", "id_empleado", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Personas", "id_empleado", c => c.String());
            //DropForeignKey("dbo.Personas", "id_empleado", "dbo.Empleados");
            //DropIndex("dbo.Personas", new[] { "id_empleado" });
            //DropPrimaryKey("dbo.DocumentacionUnidades");
            //DropPrimaryKey("dbo.ContratoPersonas");
            //AddPrimaryKey("dbo.DocumentacionUnidades", new[] { "Documentacion_id_documento", "Unidades_id_unidad" });
            //AddPrimaryKey("dbo.ContratoPersonas", new[] { "Contrato_id_contrato", "Personas_id_persona" });
            //CreateIndex("dbo.Empleados", "id_empleado");
            //AddForeignKey("dbo.Empleados", "id_empleado", "dbo.Personas", "id_persona");
            ////RenameTable(name: "dbo.DocumentacionUnidades", newName: "DocumentacionUnidades");
            //RenameTable(name: "dbo.UnidadesContrato", newName: "UnidadesContratoes");
            //RenameTable(name: "dbo.ContratoPersonas", newName: "ContratoPersonas");
        }
    }
}
