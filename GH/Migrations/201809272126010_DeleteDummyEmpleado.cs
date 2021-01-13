namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDummyEmpleado : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies");
            //DropIndex("dbo.Empleados", new[] { "Bases_id" });
            //DropColumn("dbo.Empleados", "Bases_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Empleados", "Bases_id", c => c.Int());
            CreateIndex("dbo.Empleados", "Bases_id");
            //AddForeignKey("dbo.Empleados", "Bases_id", "dbo.Dummies", "id");
        }
    }
}
