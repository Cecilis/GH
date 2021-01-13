namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoServicioDescripcion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoServicio", "descripcion", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoServicio", "descripcion");
        }
    }
}
