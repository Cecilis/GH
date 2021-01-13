namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDocumentoAplicaA : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
            AlterColumn("dbo.TipoDocumento", "aplica_a", c => c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false));
            CreateIndex("dbo.TipoDocumento", "aplica_a", unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
            AlterColumn("dbo.TipoDocumento", "aplica_a", c => c.String(nullable: false, maxLength: 1));
            CreateIndex("dbo.TipoDocumento", "aplica_a", unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
        }
    }
}
