namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRelationPersonaEmpleado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Empleados", "id_empleado", "dbo.Personas");
            DropIndex("dbo.Empleados", new[] { "id_empleado" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Empleados", "id_empleado");
            AddForeignKey("dbo.Empleados", "id_empleado", "dbo.Personas", "id_persona");
        }
    }
}
