namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpleadosGremio : DbMigration
    {
        public override void Up()
        {            
            AlterColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int(nullable: false));
            AddForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios", "id_gremio", cascadeDelete: false);            
        }
        
        public override void Down()
        {                       
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            AlterColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int());
        }
    }
}