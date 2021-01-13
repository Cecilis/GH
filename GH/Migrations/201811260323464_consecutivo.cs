namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class consecutivo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Consecutivo",
                c => new
                    {
                        id_consecutivo = c.Int(nullable: false, identity: true),
                        consecutivo = c.Long(nullable: false),
                        anio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_consecutivo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Consecutivo");
        }
    }
}
