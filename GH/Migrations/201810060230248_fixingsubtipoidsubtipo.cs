namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingsubtipoidsubtipo : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Unidades", "SubTipoUnidad_id_subtipo", "dbo.SubTipoUnidad");
            //DropIndex("dbo.Unidades", new[] { "SubTipoUnidad_id_subtipo" });
            //DropColumn("dbo.Unidades", "subtipo_unidad");
            RenameColumn(table: "dbo.Unidades", name: "SubTipoUnidad_id_subtipo", newName: "subtipo_unidad");
            AlterColumn("dbo.Unidades", "subtipo_unidad", c => c.Int(nullable: false));
            CreateIndex("dbo.Unidades", "subtipo_unidad");
            AddForeignKey("dbo.Unidades", "subtipo_unidad", "dbo.SubTipoUnidad", "id_subtipo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unidades", "subtipo_unidad", "dbo.SubTipoUnidad");
            DropIndex("dbo.Unidades", new[] { "subtipo_unidad" });
            AlterColumn("dbo.Unidades", "subtipo_unidad", c => c.Int());
            RenameColumn(table: "dbo.Unidades", name: "subtipo_unidad", newName: "SubTipoUnidad_id_subtipo");
            AddColumn("dbo.Unidades", "subtipo_unidad", c => c.Int(nullable: false));
            CreateIndex("dbo.Unidades", "SubTipoUnidad_id_subtipo");
            AddForeignKey("dbo.Unidades", "SubTipoUnidad_id_subtipo", "dbo.SubTipoUnidad", "id_subtipo");
        }
    }
}
