namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingPersonasUnidadesContratosDocumentacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContratoPersonas", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoPersonas", "id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoUnidades", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoUnidades", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoUnidades", "id_unidad", "dbo.Unidades");
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesContrato");
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesUnidad");
            DropIndex("dbo.ContratoUnidades", new[] { "Personas_id_persona" });
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasContratos");
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasPersonas");
            RenameIndex(table: "dbo.Documentacion", name: "IX_Unique_DocumentoPersonaArchivo", newName: "IX_Unique_DocumentoArchivo");
            DropTable("dbo.ContratoUnidades");
            DropTable("dbo.ContratoPersonas");
            CreateTable(
                "dbo.ContratoPersonas",
                c => new
                    {
                        Contrato_id_contrato = c.Int(nullable: false),
                        Personas_id_persona = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Contrato_id_contrato, t.Personas_id_persona })
                .ForeignKey("dbo.Contrato", t => t.Contrato_id_contrato, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.Personas_id_persona, cascadeDelete: true)
                .Index(t => t.Contrato_id_contrato)
                .Index(t => t.Personas_id_persona);
            
            CreateTable(
                "dbo.ContratoUnidades",
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
            

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ContratoPersonas",
                c => new
                    {
                        id_contrato_persona = c.Int(nullable: false, identity: true),
                        id_contrato = c.Int(nullable: false),
                        id_persona = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_contrato_persona);
            
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
            
            DropForeignKey("dbo.ContratoUnidades", "Contrato_id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoUnidades", "Unidades_id_unidad", "dbo.Unidades");
            DropForeignKey("dbo.ContratoPersonas", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoPersonas", "Contrato_id_contrato", "dbo.Contrato");
            DropIndex("dbo.ContratoUnidades", new[] { "Contrato_id_contrato" });
            DropIndex("dbo.ContratoUnidades", new[] { "Unidades_id_unidad" });
            DropIndex("dbo.ContratoPersonas", new[] { "Personas_id_persona" });
            DropIndex("dbo.ContratoPersonas", new[] { "Contrato_id_contrato" });
            DropTable("dbo.ContratoUnidades");
            DropTable("dbo.ContratoPersonas");
            RenameIndex(table: "dbo.Documentacion", name: "IX_Unique_DocumentoArchivo", newName: "IX_Unique_DocumentoPersonaArchivo");
            CreateIndex("dbo.ContratoPersonas", "id_persona", unique: true, name: "IX_Unique_ContratoPersonasPersonas");
            CreateIndex("dbo.ContratoPersonas", "id_contrato", unique: true, name: "IX_Unique_ContratoPersonasContratos");
            CreateIndex("dbo.ContratoUnidades", "Personas_id_persona");
            CreateIndex("dbo.ContratoUnidades", "id_unidad", unique: true, name: "IX_Unique_ContratoUnidadesUnidad");
            CreateIndex("dbo.ContratoUnidades", "id_contrato", unique: true, name: "IX_Unique_ContratoUnidadesContrato");
            AddForeignKey("dbo.ContratoUnidades", "id_unidad", "dbo.Unidades", "id_unidad", cascadeDelete: true);
            AddForeignKey("dbo.ContratoUnidades", "Personas_id_persona", "dbo.Personas", "id_persona");
            AddForeignKey("dbo.ContratoUnidades", "id_contrato", "dbo.Contrato", "id_contrato", cascadeDelete: true);
            AddForeignKey("dbo.ContratoPersonas", "id_persona", "dbo.Personas", "id_persona", cascadeDelete: true);
            AddForeignKey("dbo.ContratoPersonas", "id_contrato", "dbo.Contrato", "id_contrato", cascadeDelete: true);
        }
    }
}
