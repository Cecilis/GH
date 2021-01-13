namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingbase : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Base", newName: "Bases");
            RenameTable(name: "dbo.EmpleadosBase", newName: "EmpleadosBases");
            RenameColumn(table: "dbo.EmpleadosBases", name: "Base_id_base", newName: "Bases_id_base");
            RenameIndex(table: "dbo.EmpleadosBases", name: "IX_Base_id_base", newName: "IX_Bases_id_base");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EmpleadosBases", name: "IX_Bases_id_base", newName: "IX_Base_id_base");
            RenameColumn(table: "dbo.EmpleadosBases", name: "Bases_id_base", newName: "Base_id_base");
            RenameTable(name: "dbo.EmpleadosBases", newName: "EmpleadosBase");
            RenameTable(name: "dbo.Bases", newName: "Base");
        }
    }
}
