namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCotizacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "cliente_denominacion", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cotizacion", "cliente_CUIT", c => c.Long(nullable: false));
            AddColumn("dbo.Cotizacion", "cliente_direccion", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cotizacion", "cliente_telefono", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cotizacion", "cliente_mail", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cotizacion", "cliente_localidad", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Cotizacion", "locacion", c => c.String(nullable: false, maxLength: 11));
            DropColumn("dbo.Cotizacion", "cliente_nombre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cotizacion", "cliente_nombre", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Cotizacion", "locacion", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Cotizacion", "cliente_localidad");
            DropColumn("dbo.Cotizacion", "cliente_mail");
            DropColumn("dbo.Cotizacion", "cliente_telefono");
            DropColumn("dbo.Cotizacion", "cliente_direccion");
            DropColumn("dbo.Cotizacion", "cliente_CUIT");
            DropColumn("dbo.Cotizacion", "cliente_denominacion");
        }
    }
}
