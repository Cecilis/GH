namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class localidadcliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "cliente_localidad", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cotizacion", "cliente_localidad");
        }
    }
}
