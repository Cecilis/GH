namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesModelosBase : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SubTipoUnidad", new[] { "id_tipo" });
            DropIndex("dbo.SubTipoUnidad", "IX_Unique_SubTipoUnidadDescripcion");
            //RenameIndex(table: "dbo.Unidades", name: "IX_Unique_TipoUnidadSubtipoUnidad", newName: "IX_Unique_UnidadesTipoUnidadeSubTipoUnidades");
            CreateIndex("dbo.TipoDocumento", "aplica_a", unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
            CreateIndex("dbo.SubTipoUnidad", new[] { "id_tipo", "descripcion" }, unique: true, name: "IX_Unique_SubTipoUnidadesTipoUnidadDescripcion");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SubTipoUnidad", "IX_Unique_SubTipoUnidadesTipoUnidadDescripcion");
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
            //RenameIndex(table: "dbo.Unidades", name: "IX_Unique_UnidadesTipoUnidadeSubTipoUnidades", newName: "IX_Unique_TipoUnidadSubtipoUnidad");
            CreateIndex("dbo.SubTipoUnidad", "descripcion", unique: true, name: "IX_Unique_SubTipoUnidadDescripcion");
            CreateIndex("dbo.SubTipoUnidad", "id_tipo");
        }
    }
}
