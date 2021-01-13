namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CargaInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServicioItems",
                c => new
                    {
                        id_item = c.Int(nullable: false, identity: true),
                        id_tipo_servicio = c.Int(nullable: false),
                        descripcion = c.String(nullable: false, maxLength: 255),
                        precio = c.Decimal(nullable: false, storeType: "money"),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_item)
                .ForeignKey("dbo.TipoServicio", t => t.id_tipo_servicio, cascadeDelete: true)
                .Index(t => t.id_tipo_servicio)
                .Index(t => t.descripcion, unique: true);
            
            CreateTable(
                "dbo.TipoServicio",
                c => new
                    {
                        id_tipo_servicio = c.Int(nullable: false, identity: true),
                        id_servicio = c.Int(nullable: false),
                        nombre = c.String(nullable: false, maxLength: 255),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_tipo_servicio)
                .ForeignKey("dbo.Servicios", t => t.id_servicio, cascadeDelete: true)
                .Index(t => t.id_servicio)
                .Index(t => t.nombre, unique: true);
            
            CreateTable(
                "dbo.Servicios",
                c => new
                    {
                        id_servicio = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 150),
                        observaciones = c.String(maxLength: 150),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(),
                        fecha_baja = c.DateTime(),
                    })
                .PrimaryKey(t => t.id_servicio)
                .Index(t => t.nombre, unique: true);
            
            CreateTable(
                "dbo.SubTipoUnidad",
                c => new
                    {
                        id_subtipo = c.Int(nullable: false, identity: true),
                        id_tipo = c.Int(nullable: false),
                        descripcion = c.String(nullable: false, maxLength: 50),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(),
                    })
                .PrimaryKey(t => t.id_subtipo)
                .ForeignKey("dbo.TipoUnidad", t => t.id_tipo, cascadeDelete: true)
                .Index(t => t.id_tipo)
                .Index(t => t.descripcion, unique: true);
            
            CreateTable(
                "dbo.TipoUnidad",
                c => new
                    {
                        id_tipo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 50),
                        autopropulsada = c.Boolean(nullable: false),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(),
                    })
                .PrimaryKey(t => t.id_tipo)
                .Index(t => t.descripcion, unique: true);
            
            CreateTable(
                "dbo.Unidades",
                c => new
                    {
                        id_unidad = c.Int(nullable: false, identity: true),
                        tipo_unidad = c.Int(nullable: false),
                        subtipo_unidad = c.Int(nullable: false),
                        anio = c.Int(nullable: false),
                        Patente = c.String(nullable: false, maxLength: 10),
                        Marca = c.String(maxLength: 50),
                        Modelo = c.String(maxLength: 50),
                        Chasis = c.String(maxLength: 50),
                        Fecha_Compra = c.DateTime(),
                        id_base = c.Int(),
                        ultimo_service = c.Int(nullable: false),
                        Observaciones = c.String(maxLength: 150),
                        Activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(),
                        SubTipoUnidad_id_subtipo = c.Int(),
                    })
                .PrimaryKey(t => t.id_unidad)
                .ForeignKey("dbo.SubTipoUnidad", t => t.SubTipoUnidad_id_subtipo)
                .Index(t => new { t.tipo_unidad, t.subtipo_unidad }, unique: true, name: "IX_Unique_TipoUnidadesubtipoUnidad")
                .Index(t => t.SubTipoUnidad_id_subtipo);
            
            CreateTable(
                "dbo.UnidadMedida",
                c => new
                    {
                        id_unidad_medida = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 50),
                        abreviatura = c.String(nullable: false, maxLength: 10),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_unidad_medida)
                .Index(t => t.descripcion, unique: true)
                .Index(t => t.abreviatura, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unidades", "SubTipoUnidad_id_subtipo", "dbo.SubTipoUnidad");
            DropForeignKey("dbo.SubTipoUnidad", "id_tipo", "dbo.TipoUnidad");
            DropForeignKey("dbo.TipoServicio", "id_servicio", "dbo.Servicios");
            DropForeignKey("dbo.ServicioItems", "id_tipo_servicio", "dbo.TipoServicio");
            DropIndex("dbo.UnidadMedida", new[] { "abreviatura" });
            DropIndex("dbo.UnidadMedida", new[] { "descripcion" });
            DropIndex("dbo.Unidades", new[] { "SubTipoUnidad_id_subtipo" });
            DropIndex("dbo.Unidades", "IX_Unique_TipoUnidadesubtipoUnidad");
            DropIndex("dbo.TipoUnidad", new[] { "descripcion" });
            DropIndex("dbo.SubTipoUnidad", new[] { "descripcion" });
            DropIndex("dbo.SubTipoUnidad", new[] { "id_tipo" });
            DropIndex("dbo.Servicios", new[] { "nombre" });
            DropIndex("dbo.TipoServicio", new[] { "nombre" });
            DropIndex("dbo.TipoServicio", new[] { "id_servicio" });
            DropIndex("dbo.ServicioItems", new[] { "descripcion" });
            DropIndex("dbo.ServicioItems", new[] { "id_tipo_servicio" });
            DropTable("dbo.UnidadMedida");
            DropTable("dbo.Unidades");
            DropTable("dbo.TipoUnidad");
            DropTable("dbo.SubTipoUnidad");
            DropTable("dbo.Servicios");
            DropTable("dbo.TipoServicio");
            DropTable("dbo.ServicioItems");
        }
    }
}
