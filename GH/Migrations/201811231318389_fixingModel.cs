namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixingModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ServicioItems", newName: "ItemsServicio");
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            DropIndex("dbo.TipoServicio", new[] { "id_servicio" });
            DropIndex("dbo.TipoServicio", "IX_Unique_TipoServicioNombre");
            DropIndex("dbo.ItemsServicio", "IX_Unique_ServicioItemsTipoServUMedida");
            DropIndex("dbo.ItemsServicio", "IX_Unique_ServicioItemsDescripcion");
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesCotizacion");
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItem");
            CreateTable(
                "dbo.Localidad",
                c => new
                    {
                        id_localidad = c.Int(nullable: false, identity: true),
                        id_provincia = c.Int(nullable: false),
                        localidad = c.String(nullable: false, maxLength: 255),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_localidad)
                .ForeignKey("dbo.Provincias", t => t.id_provincia, cascadeDelete: true)
                .Index(t => t.id_provincia)
                .Index(t => t.localidad, unique: true, name: "IX_Unique_Localidad");
            
            CreateTable(
                "dbo.Provincias",
                c => new
                    {
                        id_provincia = c.Int(nullable: false, identity: true),
                        provincia = c.String(nullable: false, maxLength: 255),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_provincia);
            
            AddColumn("dbo.Empleados", "Bases_id_base", c => c.Int());
            AddColumn("dbo.Empleados", "Gremios_id_gremio", c => c.Int());
            AddColumn("dbo.Empleados", "Gremios_id_gremio1", c => c.Int());
            AddColumn("dbo.Empleados", "Bases_id_base1", c => c.Int());
            CreateIndex("dbo.Empleados", "Bases_id_base");
            CreateIndex("dbo.Empleados", "Gremios_id_gremio");
            CreateIndex("dbo.Empleados", "Gremios_id_gremio1");
            CreateIndex("dbo.Empleados", "Bases_id_base1");
            CreateIndex("dbo.TipoServicio", new[] { "id_servicio", "nombre" }, unique: true, name: "IX_Unique_TipoServicioServicioNombre");
            CreateIndex("dbo.ItemsServicio", new[] { "id_tipo_servicio", "id_unidad_medida", "descripcion" }, unique: true, name: "IX_Unique_ItemsServicioTipoUMDescripcion");
            CreateIndex("dbo.CotizacionDetalles", new[] { "id_cotizacion", "id_item", "id_unidad_medida" }, unique: true, name: "IX_Unique_CotizacionDetallesItemUM");
            AddForeignKey("dbo.Empleados", "Bases_id_base", "dbo.Bases", "id_base");
            AddForeignKey("dbo.Empleados", "Gremios_id_gremio1", "dbo.Gremios", "id_gremio");
            AddForeignKey("dbo.Empleados", "Bases_id_base1", "dbo.Bases", "id_base");
            AddForeignKey("dbo.Empleados", "Gremios_id_gremio", "dbo.Gremios", "id_gremio");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empleados", "Gremios_id_gremio", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "Bases_id_base1", "dbo.Bases");
            DropForeignKey("dbo.Localidad", "id_provincia", "dbo.Provincias");
            DropForeignKey("dbo.Empleados", "Gremios_id_gremio1", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "Bases_id_base", "dbo.Bases");
            DropIndex("dbo.Localidad", "IX_Unique_Localidad");
            DropIndex("dbo.Localidad", new[] { "id_provincia" });
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItemUM");
            DropIndex("dbo.ItemsServicio", "IX_Unique_ItemsServicioTipoUMDescripcion");
            DropIndex("dbo.TipoServicio", "IX_Unique_TipoServicioServicioNombre");
            DropIndex("dbo.Empleados", new[] { "Bases_id_base1" });
            DropIndex("dbo.Empleados", new[] { "Gremios_id_gremio1" });
            DropIndex("dbo.Empleados", new[] { "Gremios_id_gremio" });
            DropIndex("dbo.Empleados", new[] { "Bases_id_base" });
            DropColumn("dbo.Empleados", "Bases_id_base1");
            DropColumn("dbo.Empleados", "Gremios_id_gremio1");
            DropColumn("dbo.Empleados", "Gremios_id_gremio");
            DropColumn("dbo.Empleados", "Bases_id_base");
            DropTable("dbo.Provincias");
            DropTable("dbo.Localidad");
            CreateIndex("dbo.CotizacionDetalles", new[] { "id_item", "id_unidad_medida" }, unique: true, name: "IX_Unique_CotizacionDetallesItem");
            CreateIndex("dbo.CotizacionDetalles", "id_cotizacion", unique: true, name: "IX_Unique_CotizacionDetallesCotizacion");
            CreateIndex("dbo.ItemsServicio", "descripcion", unique: true, name: "IX_Unique_ServicioItemsDescripcion");
            CreateIndex("dbo.ItemsServicio", new[] { "id_tipo_servicio", "id_unidad_medida" }, unique: true, name: "IX_Unique_ServicioItemsTipoServUMedida");
            CreateIndex("dbo.TipoServicio", "nombre", unique: true, name: "IX_Unique_TipoServicioNombre");
            CreateIndex("dbo.TipoServicio", "id_servicio");
            AddForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios", "id_gremio", cascadeDelete: true);
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: true);
            RenameTable(name: "dbo.ItemsServicio", newName: "ServicioItems");
        }
    }
}
