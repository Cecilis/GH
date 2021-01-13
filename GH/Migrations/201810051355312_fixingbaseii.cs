namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingbaseii : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmpleadosBases", "Empleados_id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.EmpleadosBases", "Bases_id_base", "dbo.Bases");
            DropIndex("dbo.EmpleadosBases", new[] { "Empleados_id_empleado" });
            DropIndex("dbo.EmpleadosBases", new[] { "Bases_id_base" });
            CreateIndex("dbo.Empleados", "id_base");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: false);
            DropTable("dbo.EmpleadosBases");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EmpleadosBases",
                c => new
                    {
                        Empleados_id_empleado = c.Int(nullable: false),
                        Bases_id_base = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Empleados_id_empleado, t.Bases_id_base });
            
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            CreateIndex("dbo.EmpleadosBases", "Bases_id_base");
            CreateIndex("dbo.EmpleadosBases", "Empleados_id_empleado");
            AddForeignKey("dbo.EmpleadosBases", "Bases_id_base", "dbo.Bases", "id_base", cascadeDelete: true);
            AddForeignKey("dbo.EmpleadosBases", "Empleados_id_empleado", "dbo.Empleados", "id_empleado", cascadeDelete: true);
        }
    }
}
