namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTablesRelationsAndDefaultValues : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Empleados", "id_base", "dbo.Bases");
            DropForeignKey("dbo.Servicios", "Clientes_id_cliente", "dbo.Clientes");
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias");
            DropForeignKey("dbo.Empleados", "Gremios_id_gremio1", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "Bases_id_base1", "dbo.Bases");
            DropForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "Gremios_id_gremio", "dbo.Gremios");
            DropIndex("dbo.Bases", "IX_Unique_BaseNombre");
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Empleados", new[] { "Tipo_Empleado" });
            DropIndex("dbo.Empleados", new[] { "id_categoria" });
            DropIndex("dbo.Empleados", new[] { "Bases_id_base" });
            DropIndex("dbo.Empleados", new[] { "Gremios_id_gremio" });
            DropIndex("dbo.Empleados", new[] { "Gremios_id_gremio1" });
            DropIndex("dbo.Empleados", new[] { "Bases_id_base1" });
            DropIndex("dbo.Personas", new[] { "id_tipo_empleado" });
            DropIndex("dbo.Localidad", "IX_Unique_Localidad");
            DropIndex("dbo.Servicios", new[] { "Clientes_id_cliente" });
            DropIndex("dbo.TipoServicio", "IX_Unique_TipoServicioServicioNombre");
            DropColumn("dbo.Empleados", "id_base");
            DropColumn("dbo.Empleados", "Bases_id_base1");
            DropColumn("dbo.Empleados", "Gremios_id_gremio1");
            DropColumn("dbo.Empleados", "Tipo_Empleado");
            //RenameColumn(table: "dbo.Empleados", name: "Bases_id_base1", newName: "id_base");
            RenameColumn(table: "dbo.Empleados", name: "Bases_id_base", newName: "id_base");
            //RenameColumn(table: "dbo.Empleados", name: "Gremios_id_gremio1", newName: "Tipo_Empleado");
            RenameColumn(table: "dbo.Empleados", name: "Gremios_id_gremio", newName: "Tipo_Empleado");
            CreateTable(
                "dbo.Fechas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fecha_now_required = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        fecha_now_NOT_required = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        fecha_nullable_required = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        fecha_nullable_NOT_required = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Localidad", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Localidad", "fecha_baja", c => c.DateTime());
            AddColumn("dbo.Provincias", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Provincias", "fecha_baja", c => c.DateTime());
            AddColumn("dbo.CotizacionDetalles", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Bases", "nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Bases", "observaciones", c => c.String(maxLength: 100));
            AlterColumn("dbo.Bases", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Bases", "fecha_baja", c => c.DateTime());
            AlterColumn("dbo.Empleados", "id_base", c => c.Int());
            AlterColumn("dbo.Empleados", "Mail", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Empleados", "id_categoria", c => c.Int());
            AlterColumn("dbo.Empleados", "FechaAlta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int());
            AlterColumn("dbo.Personas", "id_tipo_empleado", c => c.Int(nullable: false));
            AlterColumn("dbo.Personas", "FechaAlta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Contrato", "fecha_fin", c => c.DateTime());
            AlterColumn("dbo.Clientes", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Clientes", "fecha_baja", c => c.DateTime());
            AlterColumn("dbo.Localidad", "localidad", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Provincias", "provincia", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Servicios", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.TipoServicio", "nombre", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Unidades", "Fecha_Compra", c => c.DateTime());
            AlterColumn("dbo.Unidades", "id_base", c => c.Int());
            AlterColumn("dbo.Unidades", "ultimo_service", c => c.Int());
            AlterColumn("dbo.Unidades", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Unidades", "fecha_baja", c => c.DateTime());
            AlterColumn("dbo.Documentacion", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.SubTipoUnidad", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.TipoUnidad", "fecha_alta", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.TipoUnidad", "fecha_baja", c => c.DateTime());
            CreateIndex("dbo.Bases", "nombre", unique: true, name: "IX_Unique_BaseNombre");
            CreateIndex("dbo.Empleados", "id_base");
            CreateIndex("dbo.Empleados", "Tipo_Empleado");
            CreateIndex("dbo.Empleados", "id_categoria");
            CreateIndex("dbo.Personas", "id_tipo_empleado");
            CreateIndex("dbo.Localidad", "localidad", unique: true, name: "IX_Unique_Localidad");
            CreateIndex("dbo.TipoServicio", new[] { "id_servicio", "nombre" }, unique: true, name: "IX_Unique_TipoServicioServicioNombre");
            AddForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias", "id_categoria");
            AddForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios", "id_gremio", cascadeDelete: false);
            AddForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios", "id_gremio", cascadeDelete: false);
            DropColumn("dbo.Servicios", "Clientes_id_cliente");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servicios", "Clientes_id_cliente", c => c.Int());
            DropForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios");
            DropForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias");
            DropIndex("dbo.TipoServicio", "IX_Unique_TipoServicioServicioNombre");
            DropIndex("dbo.Localidad", "IX_Unique_Localidad");
            DropIndex("dbo.Personas", new[] { "id_tipo_empleado" });
            DropIndex("dbo.Empleados", new[] { "id_categoria" });
            DropIndex("dbo.Empleados", new[] { "Tipo_Empleado" });
            DropIndex("dbo.Empleados", new[] { "id_base" });
            DropIndex("dbo.Bases", "IX_Unique_BaseNombre");
            AlterColumn("dbo.TipoUnidad", "fecha_baja", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TipoUnidad", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubTipoUnidad", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Documentacion", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidades", "fecha_baja", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidades", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidades", "ultimo_service", c => c.Int(nullable: false));
            AlterColumn("dbo.Unidades", "id_base", c => c.Int(nullable: false));
            AlterColumn("dbo.Unidades", "Fecha_Compra", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TipoServicio", "nombre", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Servicios", "fecha_alta", c => c.DateTime());
            AlterColumn("dbo.Provincias", "provincia", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Localidad", "localidad", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Clientes", "fecha_baja", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Clientes", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contrato", "fecha_fin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Personas", "FechaAlta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Personas", "id_tipo_empleado", c => c.Int());
            AlterColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int());
            AlterColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int());
            AlterColumn("dbo.Empleados", "FechaAlta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Empleados", "id_categoria", c => c.Int(nullable: false));
            AlterColumn("dbo.Empleados", "Mail", c => c.String(maxLength: 50));
            AlterColumn("dbo.Empleados", "id_base", c => c.Int(nullable: false));
            AlterColumn("dbo.Bases", "fecha_baja", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Bases", "fecha_alta", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Bases", "observaciones", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Bases", "nombre", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.CotizacionDetalles", "fecha_alta");
            DropColumn("dbo.Provincias", "fecha_baja");
            DropColumn("dbo.Provincias", "fecha_alta");
            DropColumn("dbo.Localidad", "fecha_baja");
            DropColumn("dbo.Localidad", "fecha_alta");
            DropTable("dbo.Fechas");
            RenameColumn(table: "dbo.Empleados", name: "Tipo_Empleado", newName: "Gremios_id_gremio");
            RenameColumn(table: "dbo.Empleados", name: "Tipo_Empleado", newName: "Gremios_id_gremio1");
            RenameColumn(table: "dbo.Empleados", name: "id_base", newName: "Bases_id_base");
            RenameColumn(table: "dbo.Empleados", name: "id_base", newName: "Bases_id_base1");
            AddColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int(nullable: false));
            AddColumn("dbo.Empleados", "Tipo_Empleado", c => c.Int(nullable: false));
            AddColumn("dbo.Empleados", "id_base", c => c.Int(nullable: false));
            AddColumn("dbo.Empleados", "id_base", c => c.Int(nullable: false));
            CreateIndex("dbo.TipoServicio", new[] { "id_servicio", "nombre" }, unique: true, name: "IX_Unique_TipoServicioServicioNombre");
            CreateIndex("dbo.Servicios", "Clientes_id_cliente");
            CreateIndex("dbo.Localidad", "localidad", unique: true, name: "IX_Unique_Localidad");
            CreateIndex("dbo.Personas", "id_tipo_empleado");
            CreateIndex("dbo.Empleados", "Bases_id_base1");
            CreateIndex("dbo.Empleados", "Gremios_id_gremio1");
            CreateIndex("dbo.Empleados", "Gremios_id_gremio");
            CreateIndex("dbo.Empleados", "Bases_id_base");
            CreateIndex("dbo.Empleados", "id_categoria");
            CreateIndex("dbo.Empleados", "Tipo_Empleado");
            CreateIndex("dbo.Empleados", "id_base");
            CreateIndex("dbo.Bases", "nombre", unique: true, name: "IX_Unique_BaseNombre");
            AddForeignKey("dbo.Empleados", "Gremios_id_gremio", "dbo.Gremios", "id_gremio");
            AddForeignKey("dbo.Personas", "id_tipo_empleado", "dbo.Gremios", "id_gremio");
            AddForeignKey("dbo.Empleados", "Gremios_id_gremio1", "dbo.Gremios", "id_gremio");
            AddForeignKey("dbo.Empleados", "id_categoria", "dbo.Categorias", "id_categoria", cascadeDelete: false);
            AddForeignKey("dbo.Empleados", "Tipo_Empleado", "dbo.Gremios", "id_gremio", cascadeDelete: false);
            AddForeignKey("dbo.Servicios", "Clientes_id_cliente", "dbo.Clientes", "id_cliente");
            AddForeignKey("dbo.Empleados", "id_base", "dbo.Bases", "id_base", cascadeDelete: false);
        }
    }
}
