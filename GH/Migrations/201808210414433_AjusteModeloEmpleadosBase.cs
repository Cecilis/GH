namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteModeloEmpleadosBase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BasesEmpleados", "Bases_id_base", "dbo.Bases");
            DropForeignKey("dbo.BasesEmpleados", "Empleados_id_empleado", "dbo.Empleados");
            DropIndex("dbo.BasesEmpleados", new[] { "Bases_id_base" });
            DropIndex("dbo.BasesEmpleados", new[] { "Empleados_id_empleado" });
            AddColumn("dbo.Empleados", "id_base", c => c.Int(nullable: false));
            CreateIndex("dbo.Empleados", "id_base");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
            DropTable("dbo.BasesEmpleados");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BasesEmpleados",
                c => new
                    {
                        Bases_id_base = c.Int(nullable: false),
                        Empleados_id_empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bases_id_base, t.Empleados_id_empleado });
            
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropColumn("dbo.Empleados", "id_base");
            CreateIndex("dbo.BasesEmpleados", "Empleados_id_empleado");
            CreateIndex("dbo.BasesEmpleados", "Bases_id_base");
            AddForeignKey("dbo.BasesEmpleados", "Empleados_id_empleado", "dbo.Empleados", "id_empleado", cascadeDelete: true);
            AddForeignKey("dbo.BasesEmpleados", "Bases_id_base", "dbo.Bases", "id_base", cascadeDelete: true);
        }
    }
}
