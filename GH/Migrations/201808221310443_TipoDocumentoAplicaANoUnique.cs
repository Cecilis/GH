namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDocumentoAplicaANoUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.TipoDocumento", "aplica_a", unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
        }
    }
}
