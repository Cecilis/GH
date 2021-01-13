namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesCotizacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "id_usuario_alta", c => c.Int(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion_alta", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Cotizacion", "id_tipo_servicio", c => c.Int(nullable: false));
            AddColumn("dbo.Cotizacion", "estatus_evaluacion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion_evaluacion", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Cotizacion", "fecha_evaluacion", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Cotizacion", "id_usuario_evaluacion", c => c.Int(nullable: false));
            AddColumn("dbo.Cotizacion", "estatus_decision", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion_decision", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Cotizacion", "id_usuario_decision", c => c.Int(nullable: false));
            CreateIndex("dbo.Cotizacion", "id_tipo_servicio");
            AddForeignKey("dbo.Cotizacion", "id_tipo_servicio", "dbo.TipoServicio", "id_tipo_servicio", cascadeDelete: true);
            DropColumn("dbo.Cotizacion", "observacion");
            DropColumn("dbo.Cotizacion", "cotizacion_aceptada");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cotizacion", "cotizacion_aceptada", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion", c => c.String(nullable: false, maxLength: 500));
            DropForeignKey("dbo.Cotizacion", "id_tipo_servicio", "dbo.TipoServicio");
            DropIndex("dbo.Cotizacion", new[] { "id_tipo_servicio" });
            DropColumn("dbo.Cotizacion", "id_usuario_decision");
            DropColumn("dbo.Cotizacion", "observacion_decision");
            DropColumn("dbo.Cotizacion", "estatus_decision");
            DropColumn("dbo.Cotizacion", "id_usuario_evaluacion");
            DropColumn("dbo.Cotizacion", "fecha_evaluacion");
            DropColumn("dbo.Cotizacion", "observacion_evaluacion");
            DropColumn("dbo.Cotizacion", "estatus_evaluacion");
            DropColumn("dbo.Cotizacion", "id_tipo_servicio");
            DropColumn("dbo.Cotizacion", "observacion_alta");
            DropColumn("dbo.Cotizacion", "id_usuario_alta");
        }
    }
}
