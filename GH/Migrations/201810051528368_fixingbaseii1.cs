namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingbaseii1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Unidades", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.Personas", "Contrato_id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Personas", new[] { "Contrato_id_contrato" });
            DropIndex("dbo.Unidades", new[] { "Personas_id_persona" });
            CreateTable(
                "dbo.PersonasContrato",
                c => new
                    {
                        Personas_id_persona = c.Int(nullable: false),
                        Contrato_id_contrato = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Personas_id_persona, t.Contrato_id_contrato })
                .ForeignKey("dbo.Personas", t => t.Personas_id_persona, cascadeDelete: true)
                .ForeignKey("dbo.Contrato", t => t.Contrato_id_contrato, cascadeDelete: true)
                .Index(t => t.Personas_id_persona)
                .Index(t => t.Contrato_id_contrato);
            
            AddColumn("dbo.Bases", "Empleados_id_empleado", c => c.Int());
            CreateIndex("dbo.Bases", "Empleados_id_empleado");
            AddForeignKey("dbo.Bases", "Empleados_id_empleado", "dbo.Empleados", "id_empleado");
            DropColumn("dbo.Personas", "Contrato_id_contrato");
            DropColumn("dbo.Unidades", "Personas_id_persona");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Unidades", "Personas_id_persona", c => c.Int());
            AddColumn("dbo.Personas", "Contrato_id_contrato", c => c.Int());
            DropForeignKey("dbo.Bases", "Empleados_id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.PersonasContrato", "Contrato_id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.PersonasContrato", "Personas_id_persona", "dbo.Personas");
            DropIndex("dbo.PersonasContrato", new[] { "Contrato_id_contrato" });
            DropIndex("dbo.PersonasContrato", new[] { "Personas_id_persona" });
            DropIndex("dbo.Bases", new[] { "Empleados_id_empleado" });
            DropColumn("dbo.Bases", "Empleados_id_empleado");
            DropTable("dbo.PersonasContrato");
            CreateIndex("dbo.Unidades", "Personas_id_persona");
            CreateIndex("dbo.Personas", "Contrato_id_contrato");
            CreateIndex("dbo.Empleados", "id_base");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
            AddForeignKey("dbo.Personas", "Contrato_id_contrato", "dbo.Contrato", "id_contrato");
            AddForeignKey("dbo.Unidades", "Personas_id_persona", "dbo.Personas", "id_persona");
        }
    }
}
