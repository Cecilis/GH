namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Documentacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentosPersona", "id_persona", "dbo.Personas");
            DropForeignKey("dbo.DocumentosPersona", "id_tipo_documento", "dbo.TipoDocumento");
            DropForeignKey("dbo.DocumentosUnidad", "id_tipo_documento", "dbo.TipoDocumento");
            DropForeignKey("dbo.DocumentosUnidad", "id_Unidad", "dbo.Unidades");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentosUnidadTipoDocumentos");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentosUnidadUnidades");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentoUnidadArchivo");
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
            DropIndex("dbo.DocumentosPersona", "IX_Unique_DocumentosPersonaTipoDocumentos");
            DropIndex("dbo.DocumentosPersona", "IX_Unique_DocumentosPersonaPersonas");
            DropIndex("dbo.DocumentosPersona", "IX_Unique_DocumentoPersonaArchivo");
            CreateTable(
                "dbo.Documentacion",
                c => new
                    {
                        id_documento = c.Int(nullable: false, identity: true),
                        id_tipo_documento = c.Int(nullable: false),
                        documento = c.String(nullable: false, maxLength: 500),
                        fecha_alta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_documento)
                .ForeignKey("dbo.TipoDocumento", t => t.id_tipo_documento, cascadeDelete: true)
                .Index(t => t.id_tipo_documento)
                .Index(t => t.documento, unique: true, name: "IX_Unique_DocumentoPersonaArchivo");
            
            CreateTable(
                "dbo.DocumentacionPersonas",
                c => new
                    {
                        Documentacion_id_documento = c.Int(nullable: false),
                        Personas_id_persona = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Documentacion_id_documento, t.Personas_id_persona })
                .ForeignKey("dbo.Documentacion", t => t.Documentacion_id_documento, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.Personas_id_persona, cascadeDelete: true)
                .Index(t => t.Documentacion_id_documento)
                .Index(t => t.Personas_id_persona);
            
            CreateTable(
                "dbo.DocumentacionUnidades",
                c => new
                    {
                        Documentacion_id_documento = c.Int(nullable: false),
                        Unidades_id_unidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Documentacion_id_documento, t.Unidades_id_unidad })
                .ForeignKey("dbo.Documentacion", t => t.Documentacion_id_documento, cascadeDelete: true)
                .ForeignKey("dbo.Unidades", t => t.Unidades_id_unidad, cascadeDelete: true)
                .Index(t => t.Documentacion_id_documento)
                .Index(t => t.Unidades_id_unidad);
            
            DropTable("dbo.DocumentosUnidad");
            DropTable("dbo.DocumentosPersona");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentosPersona",
                c => new
                    {
                        id_documento_Persona = c.Int(nullable: false, identity: true),
                        id_tipo_documento = c.Int(nullable: false),
                        id_persona = c.Int(nullable: false),
                        ruta = c.String(nullable: false, maxLength: 500),
                        archivo = c.String(nullable: false, maxLength: 255),
                        fecha_documento = c.DateTime(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_documento_Persona);
            
            CreateTable(
                "dbo.DocumentosUnidad",
                c => new
                    {
                        id_documento_Unidad = c.Int(nullable: false, identity: true),
                        id_tipo_documento = c.Int(nullable: false),
                        id_Unidad = c.Int(nullable: false),
                        ruta = c.String(nullable: false),
                        archivo = c.String(nullable: false, maxLength: 255),
                        fecha_documento = c.DateTime(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_documento_Unidad);
            
            DropForeignKey("dbo.DocumentacionUnidades", "Unidades_id_unidad", "dbo.Unidades");
            DropForeignKey("dbo.DocumentacionUnidades", "Documentacion_id_documento", "dbo.Documentacion");
            DropForeignKey("dbo.Documentacion", "id_tipo_documento", "dbo.TipoDocumento");
            DropForeignKey("dbo.DocumentacionPersonas", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.DocumentacionPersonas", "Documentacion_id_documento", "dbo.Documentacion");
            DropIndex("dbo.DocumentacionUnidades", new[] { "Unidades_id_unidad" });
            DropIndex("dbo.DocumentacionUnidades", new[] { "Documentacion_id_documento" });
            DropIndex("dbo.DocumentacionPersonas", new[] { "Personas_id_persona" });
            DropIndex("dbo.DocumentacionPersonas", new[] { "Documentacion_id_documento" });
            DropIndex("dbo.Documentacion", "IX_Unique_DocumentoPersonaArchivo");
            DropIndex("dbo.Documentacion", new[] { "id_tipo_documento" });
            DropTable("dbo.DocumentacionUnidades");
            DropTable("dbo.DocumentacionPersonas");
            DropTable("dbo.Documentacion");
            CreateIndex("dbo.DocumentosPersona", "archivo", unique: true, name: "IX_Unique_DocumentoPersonaArchivo");
            CreateIndex("dbo.DocumentosPersona", "id_persona", unique: true, name: "IX_Unique_DocumentosPersonaPersonas");
            CreateIndex("dbo.DocumentosPersona", "id_tipo_documento", unique: true, name: "IX_Unique_DocumentosPersonaTipoDocumentos");
            CreateIndex("dbo.TipoDocumento", "aplica_a", unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
            CreateIndex("dbo.DocumentosUnidad", "archivo", unique: true, name: "IX_Unique_DocumentoUnidadArchivo");
            CreateIndex("dbo.DocumentosUnidad", "id_Unidad", unique: true, name: "IX_Unique_DocumentosUnidadUnidades");
            CreateIndex("dbo.DocumentosUnidad", "id_tipo_documento", unique: true, name: "IX_Unique_DocumentosUnidadTipoDocumentos");
            AddForeignKey("dbo.DocumentosUnidad", "id_Unidad", "dbo.Unidades", "id_unidad", cascadeDelete: true);
            AddForeignKey("dbo.DocumentosUnidad", "id_tipo_documento", "dbo.TipoDocumento", "id_tipo_documento", cascadeDelete: true);
            AddForeignKey("dbo.DocumentosPersona", "id_tipo_documento", "dbo.TipoDocumento", "id_tipo_documento", cascadeDelete: true);
            AddForeignKey("dbo.DocumentosPersona", "id_persona", "dbo.Personas", "id_persona", cascadeDelete: true);
        }
    }
}
