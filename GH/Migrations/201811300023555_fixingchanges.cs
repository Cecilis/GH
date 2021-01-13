namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "estatus_aprobacion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion_aprobacion", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Cotizacion", "fecha_aprobacion", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Cotizacion", "id_usuario_aprobacion", c => c.Int(nullable: false));
            DropColumn("dbo.Cotizacion", "estatus_decision");
            DropColumn("dbo.Cotizacion", "observacion_decision");
            DropColumn("dbo.Cotizacion", "fecha_decision");
            DropColumn("dbo.Cotizacion", "id_usuario_decision");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cotizacion", "id_usuario_decision", c => c.Int(nullable: false));
            AddColumn("dbo.Cotizacion", "fecha_decision", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cotizacion", "observacion_decision", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Cotizacion", "estatus_decision", c => c.Boolean(nullable: false));
            DropColumn("dbo.Cotizacion", "id_usuario_aprobacion");
            DropColumn("dbo.Cotizacion", "fecha_aprobacion");
            DropColumn("dbo.Cotizacion", "observacion_aprobacion");
            DropColumn("dbo.Cotizacion", "estatus_aprobacion");
        }
    }
}
