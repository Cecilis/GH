namespace GH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelosBaseCotizacion : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Servicios", name: "IX_nombre", newName: "IX_Unique_ServiciosNombre");
            RenameIndex(table: "dbo.TipoServicio", name: "IX_nombre", newName: "IX_Unique_TipoServicioNombre");
            RenameIndex(table: "dbo.SubTipoUnidad", name: "IX_descripcion", newName: "IX_Unique_SubTipoUnidadDescripcion");
            RenameIndex(table: "dbo.TipoUnidad", name: "IX_descripcion", newName: "IX_Unique_TipoUnidadDescripcion");
            RenameIndex(table: "dbo.UnidadMedida", name: "IX_descripcion", newName: "IX_Unique_UnidadMedidaDescripcion");
            RenameIndex(table: "dbo.UnidadMedida", name: "IX_abreviatura", newName: "IX_Unique_UnidadMedidaAbreviatura");
            CreateTable(
                "dbo.CotizacionDetalles",
                c => new
                    {
                        id_cotizacion_detalle = c.Int(nullable: false, identity: true),
                        id_cotizacion = c.Int(nullable: false),
                        id_item = c.Int(nullable: false),
                        descripcion = c.String(nullable: false, maxLength: 500),
                        id_unidad_medida = c.Int(nullable: false),
                        precio = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.id_cotizacion_detalle)
                .ForeignKey("dbo.Cotizacion", t => t.id_cotizacion, cascadeDelete: true)
                .ForeignKey("dbo.UnidadMedida", t => t.id_unidad_medida, cascadeDelete: true)
                .Index(t => t.id_cotizacion, unique: true, name: "IX_Unique_CotizacionDetallesCotizacion")
                .Index(t => t.id_item, unique: true, name: "IX_Unique_CotizacionDetallesItem")
                .Index(t => t.id_unidad_medida);
            
            CreateTable(
                "dbo.Cotizacion",
                c => new
                    {
                        id_cotizacion = c.Int(nullable: false, identity: true),
                        id_cliente = c.Int(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        observacion = c.String(nullable: false, maxLength: 500),
                        estatus = c.String(nullable: false, maxLength: 1),
                        anio = c.Int(nullable: false),
                        nro_cotizacion = c.Long(nullable: false),
                        revision = c.Short(nullable: false),
                        duracion_oferta = c.Int(nullable: false),
                        vigencia = c.DateTime(nullable: false),
                        condicion_pago = c.Int(nullable: false),
                        cliente_nombre = c.String(nullable: false, maxLength: 255),
                        locacion = c.String(nullable: false, maxLength: 255),
                        contacto = c.String(nullable: false, maxLength: 255),
                        telefono = c.String(nullable: false, maxLength: 15),
                        tipo_servicio_descripcion = c.String(nullable: false, maxLength: 125),
                        servicio_descripcion = c.String(nullable: false, maxLength: 500),
                        cotizacion_aceptada = c.Boolean(nullable: false),
                        fecha_decision = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_cotizacion)
                .ForeignKey("dbo.Clientes", t => t.id_cliente, cascadeDelete: true)
                .Index(t => t.id_cliente);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        id_cliente = c.Int(nullable: false, identity: true),
                        Denominacion = c.String(nullable: false, maxLength: 100),
                        CUIT = c.Long(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(nullable: false, maxLength: 100),
                        Mail = c.String(maxLength: 100),
                        Localidad = c.String(nullable: false, maxLength: 100),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_cliente)
                .Index(t => t.Denominacion, unique: true, name: "IX_Unique_ClientesDenominacion");
            
            CreateTable(
                "dbo.Contrato",
                c => new
                    {
                        id_contrato = c.Int(nullable: false, identity: true),
                        id_cotizacion = c.Int(nullable: false),
                        cuit_facturacion = c.String(nullable: false, maxLength: 100),
                        fecha_inicio = c.DateTime(nullable: false),
                        fecha_fin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_contrato)
                .ForeignKey("dbo.Cotizacion", t => t.id_contrato)
                .Index(t => t.id_contrato);
            
            CreateTable(
                "dbo.ContratoPersonas",
                c => new
                    {
                        id_contrato_persona = c.Int(nullable: false, identity: true),
                        id_contrato = c.Int(nullable: false),
                        id_persona = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_contrato_persona)
                .ForeignKey("dbo.Contrato", t => t.id_contrato, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.id_persona, cascadeDelete: true)
                .Index(t => t.id_contrato, unique: true, name: "IX_Unique_ContratoPersonasContratos")
                .Index(t => t.id_persona, unique: true, name: "IX_Unique_ContratoPersonasPersonas");
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        id_persona = c.Int(nullable: false, identity: true),
                        id_tipo_persona = c.Int(nullable: false),
                        id_empleado = c.Int(nullable: false),
                        Legajo = c.Int(),
                        DNI = c.Int(nullable: false),
                        Apellido = c.String(nullable: false, maxLength: 50),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Mail = c.String(nullable: false, maxLength: 50),
                        id_tipo_empleado = c.Int(),
                        id_categoria = c.Int(),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaBaja = c.DateTime(),
                    })
                .PrimaryKey(t => t.id_persona)
                .ForeignKey("dbo.TipoPersona", t => t.id_tipo_persona, cascadeDelete: true)
                .Index(t => t.id_tipo_persona)
                .Index(t => t.DNI, unique: true, name: "IX_Unique_PersonasDNI");
            
            CreateTable(
                "dbo.ContratoUnidades",
                c => new
                    {
                        id_contrato_unidad = c.Int(nullable: false, identity: true),
                        id_contrato = c.Int(nullable: false),
                        id_unidad = c.Int(nullable: false),
                        Personas_id_persona = c.Int(),
                    })
                .PrimaryKey(t => t.id_contrato_unidad)
                .ForeignKey("dbo.Contrato", t => t.id_contrato, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.Personas_id_persona)
                .ForeignKey("dbo.Unidades", t => t.id_unidad, cascadeDelete: true)
                .Index(t => t.id_contrato, unique: true, name: "IX_Unique_ContratoUnidadesContrato")
                .Index(t => t.id_unidad, unique: true, name: "IX_Unique_ContratoUnidadesUnidad")
                .Index(t => t.Personas_id_persona);
            
            CreateTable(
                "dbo.DocumentosUnidad",
                c => new
                    {
                        id_documento_Unidad = c.Int(nullable: false, identity: true),
                        id_tipo_documento = c.Int(nullable: false),
                        id_Unidad = c.Int(nullable: false),
                        ruta = c.String(nullable: false),
                        archivo = c.String(nullable: false, maxLength: 255),
                        fecha_documento = c.DateTime(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_documento_Unidad)
                .ForeignKey("dbo.TipoDocumento", t => t.id_tipo_documento, cascadeDelete: true)
                .ForeignKey("dbo.Unidades", t => t.id_Unidad, cascadeDelete: true)
                .Index(t => t.id_tipo_documento, unique: true, name: "IX_Unique_DocumentosUnidadTipoDocumentos")
                .Index(t => t.id_Unidad, unique: true, name: "IX_Unique_DocumentosUnidadUnidades")
                .Index(t => t.archivo, unique: true, name: "IX_Unique_DocumentoUnidadArchivo");
            
            CreateTable(
                "dbo.TipoDocumento",
                c => new
                    {
                        id_tipo_documento = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 255),
                        aplica_a = c.String(nullable: false, maxLength: 1),
                        vencimiento = c.Boolean(nullable: false),
                        nro_dias_vencimiento = c.Int(nullable: false),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_tipo_documento)
                .Index(t => t.nombre, unique: true, name: "IX_Unique_TipoDocumentoNombre")
                .Index(t => t.aplica_a, unique: true, name: "IX_Unique_TipoDocumentoAplicaA");
            
            CreateTable(
                "dbo.Documentacion",
                c => new
                    {
                        id_documento_Persona = c.Int(nullable: false, identity: true),
                        id_tipo_documento = c.Int(nullable: false),
                        id_persona = c.Int(nullable: false),
                        ruta = c.String(nullable: false, maxLength: 500),
                        archivo = c.String(nullable: false, maxLength: 255),
                        fecha_documento = c.DateTime(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_documento_Persona)
                .ForeignKey("dbo.Personas", t => t.id_persona, cascadeDelete: true)
                .ForeignKey("dbo.TipoDocumento", t => t.id_tipo_documento, cascadeDelete: true)
                .Index(t => t.id_tipo_documento, unique: true, name: "IX_Unique_DocumentosPersonaTipoDocumentos")
                .Index(t => t.id_persona, unique: true, name: "IX_Unique_DocumentosPersonaPersonas")
                .Index(t => t.archivo, unique: true, name: "IX_Unique_DocumentoPersonaArchivo");
            
            CreateTable(
                "dbo.Empleados",
                c => new
                    {
                        id_empleado = c.Int(nullable: false, identity: true),
                        Legajo = c.Int(nullable: false),
                        DNI = c.Int(nullable: false),
                        Apellido = c.String(nullable: false, maxLength: 50),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Mail = c.String(maxLength: 50),
                        Tipo_Empleado = c.Int(nullable: false),
                        id_categoria = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaBaja = c.DateTime(),
                    })
                .PrimaryKey(t => t.id_empleado)
                .ForeignKey("dbo.Personas", t => t.id_empleado)
                .Index(t => t.id_empleado)
                .Index(t => t.DNI, unique: true, name: "IX_Unique_EmpleadosDNI")
                .Index(t => t.Nombre, unique: true, name: "IX_Unique_EmpleadosNombre");
            
            CreateTable(
                "dbo.Bases",
                c => new
                    {
                        id_base = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 255),
                        observaciones = c.String(nullable: false, maxLength: 100),
                        activo = c.Boolean(nullable: false),
                        fecha_alta = c.DateTime(nullable: false),
                        fecha_baja = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_base)
                .Index(t => t.nombre, unique: true, name: "IX_Unique_BaseNombre");
            
            CreateTable(
                "dbo.TipoPersona",
                c => new
                    {
                        id_tipo_persona = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 50),
                        activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_tipo_persona)
                .Index(t => t.descripcion, unique: true, name: "IX_Unique_TipoPersonaDescripcion");
            
            CreateTable(
                "dbo.BasesEmpleados",
                c => new
                    {
                        Bases_id_base = c.Int(nullable: false),
                        Empleados_id_empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bases_id_base, t.Empleados_id_empleado })
                .ForeignKey("dbo.Bases", t => t.Bases_id_base, cascadeDelete: true)
                .ForeignKey("dbo.Empleados", t => t.Empleados_id_empleado, cascadeDelete: true)
                .Index(t => t.Bases_id_base)
                .Index(t => t.Empleados_id_empleado);
            
            AddColumn("dbo.Servicios", "Clientes_id_cliente", c => c.Int());
            AlterColumn("dbo.TipoUnidad", "fecha_baja", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidades", "Fecha_Compra", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidades", "id_base", c => c.Int(nullable: false));
            AlterColumn("dbo.Unidades", "fecha_baja", c => c.DateTime(nullable: false));
            CreateIndex("dbo.ServicioItems", "id_item");
            CreateIndex("dbo.Servicios", "Clientes_id_cliente");
            AddForeignKey("dbo.Servicios", "Clientes_id_cliente", "dbo.Clientes", "id_cliente");
            AddForeignKey("dbo.ServicioItems", "id_item", "dbo.CotizacionDetalles", "id_cotizacion_detalle");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServicioItems", "id_item", "dbo.CotizacionDetalles");
            DropForeignKey("dbo.CotizacionDetalles", "id_unidad_medida", "dbo.UnidadMedida");
            DropForeignKey("dbo.CotizacionDetalles", "id_cotizacion", "dbo.Cotizacion");
            DropForeignKey("dbo.Contrato", "id_contrato", "dbo.Cotizacion");
            DropForeignKey("dbo.ContratoPersonas", "id_persona", "dbo.Personas");
            DropForeignKey("dbo.Personas", "id_tipo_persona", "dbo.TipoPersona");
            DropForeignKey("dbo.Empleados", "id_empleado", "dbo.Personas");
            DropForeignKey("dbo.BasesEmpleados", "Empleados_id_empleado", "dbo.Empleados");
            DropForeignKey("dbo.BasesEmpleados", "Bases_id_base", "dbo.Bases");
            DropForeignKey("dbo.DocumentosUnidad", "id_Unidad", "dbo.Unidades");
            DropForeignKey("dbo.DocumentosUnidad", "id_tipo_documento", "dbo.TipoDocumento");
            DropForeignKey("dbo.Documentacion", "id_tipo_documento", "dbo.TipoDocumento");
            DropForeignKey("dbo.Documentacion", "id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoUnidades", "id_unidad", "dbo.Unidades");
            DropForeignKey("dbo.ContratoUnidades", "Personas_id_persona", "dbo.Personas");
            DropForeignKey("dbo.ContratoUnidades", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.ContratoPersonas", "id_contrato", "dbo.Contrato");
            DropForeignKey("dbo.Servicios", "Clientes_id_cliente", "dbo.Clientes");
            DropForeignKey("dbo.Cotizacion", "id_cliente", "dbo.Clientes");
            DropIndex("dbo.BasesEmpleados", new[] { "Empleados_id_empleado" });
            DropIndex("dbo.BasesEmpleados", new[] { "Bases_id_base" });
            DropIndex("dbo.TipoPersona", "IX_Unique_TipoPersonaDescripcion");
            DropIndex("dbo.Bases", "IX_Unique_BaseNombre");
            DropIndex("dbo.Empleados", "IX_Unique_EmpleadosNombre");
            DropIndex("dbo.Empleados", "IX_Unique_EmpleadosDNI");
            DropIndex("dbo.Empleados", new[] { "id_empleado" });
            DropIndex("dbo.Documentacion", "IX_Unique_DocumentoPersonaArchivo");
            DropIndex("dbo.Documentacion", "IX_Unique_DocumentosPersonaPersonas");
            DropIndex("dbo.Documentacion", "IX_Unique_DocumentosPersonaTipoDocumentos");
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoAplicaA");
            DropIndex("dbo.TipoDocumento", "IX_Unique_TipoDocumentoNombre");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentoUnidadArchivo");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentosUnidadUnidades");
            DropIndex("dbo.DocumentosUnidad", "IX_Unique_DocumentosUnidadTipoDocumentos");
            DropIndex("dbo.ContratoUnidades", new[] { "Personas_id_persona" });
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesUnidad");
            DropIndex("dbo.ContratoUnidades", "IX_Unique_ContratoUnidadesContrato");
            DropIndex("dbo.Personas", "IX_Unique_PersonasDNI");
            DropIndex("dbo.Personas", new[] { "id_tipo_persona" });
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasPersonas");
            DropIndex("dbo.ContratoPersonas", "IX_Unique_ContratoPersonasContratos");
            DropIndex("dbo.Contrato", new[] { "id_contrato" });
            DropIndex("dbo.Servicios", new[] { "Clientes_id_cliente" });
            DropIndex("dbo.Clientes", "IX_Unique_ClientesDenominacion");
            DropIndex("dbo.Cotizacion", new[] { "id_cliente" });
            DropIndex("dbo.CotizacionDetalles", new[] { "id_unidad_medida" });
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesItem");
            DropIndex("dbo.CotizacionDetalles", "IX_Unique_CotizacionDetallesCotizacion");
            DropIndex("dbo.ServicioItems", new[] { "id_item" });
            AlterColumn("dbo.Unidades", "fecha_baja", c => c.DateTime());
            AlterColumn("dbo.Unidades", "id_base", c => c.Int());
            AlterColumn("dbo.Unidades", "Fecha_Compra", c => c.DateTime());
            AlterColumn("dbo.TipoUnidad", "fecha_baja", c => c.DateTime());
            DropColumn("dbo.Servicios", "Clientes_id_cliente");
            DropTable("dbo.BasesEmpleados");
            DropTable("dbo.TipoPersona");
            DropTable("dbo.Bases");
            DropTable("dbo.Empleados");
            DropTable("dbo.Documentacion");
            DropTable("dbo.TipoDocumento");
            DropTable("dbo.DocumentosUnidad");
            DropTable("dbo.ContratoUnidades");
            DropTable("dbo.Personas");
            DropTable("dbo.ContratoPersonas");
            DropTable("dbo.Contrato");
            DropTable("dbo.Clientes");
            DropTable("dbo.Cotizacion");
            DropTable("dbo.CotizacionDetalles");
            RenameIndex(table: "dbo.UnidadMedida", name: "IX_Unique_UnidadMedidaAbreviatura", newName: "IX_abreviatura");
            RenameIndex(table: "dbo.UnidadMedida", name: "IX_Unique_UnidadMedidaDescripcion", newName: "IX_descripcion");
            RenameIndex(table: "dbo.TipoUnidad", name: "IX_Unique_TipoUnidadDescripcion", newName: "IX_descripcion");
            RenameIndex(table: "dbo.SubTipoUnidad", name: "IX_Unique_SubTipoUnidadDescripcion", newName: "IX_descripcion");
            RenameIndex(table: "dbo.TipoServicio", name: "IX_Unique_TipoServicioNombre", newName: "IX_nombre");
            RenameIndex(table: "dbo.Servicios", name: "IX_Unique_ServiciosNombre", newName: "IX_nombre");
        }
    }
}
