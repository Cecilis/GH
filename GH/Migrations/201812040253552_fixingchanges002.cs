namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingchanges002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "cliente_id_localidad", c => c.Int(nullable: false));
            AddColumn("dbo.Cotizacion", "id_localidad", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.Clientes", "id_localidad", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Cotizacion", "id_localidad");
            CreateIndex("dbo.Clientes", "id_localidad");
            AddForeignKey("dbo.Clientes", "id_localidad", "dbo.Localidad", "id_localidad", cascadeDelete: false);
            AddForeignKey("dbo.Cotizacion", "id_localidad", "dbo.Localidad", "id_localidad", cascadeDelete: false);
            DropColumn("dbo.Cotizacion", "cliente_localidad");
            DropColumn("dbo.Cotizacion", "locacion");
            DropColumn("dbo.Clientes", "Localidad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Localidad", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Cotizacion", "locacion", c => c.String(nullable: false, maxLength: 11));
            AddColumn("dbo.Cotizacion", "cliente_localidad", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.Cotizacion", "id_localidad", "dbo.Localidad");
            DropForeignKey("dbo.Clientes", "id_localidad", "dbo.Localidad");
            DropIndex("dbo.Clientes", new[] { "id_localidad" });
            DropIndex("dbo.Cotizacion", new[] { "id_localidad" });
            DropColumn("dbo.Clientes", "id_localidad");
            DropColumn("dbo.Cotizacion", "id_localidad");
            DropColumn("dbo.Cotizacion", "cliente_id_localidad");
        }
    }
}
