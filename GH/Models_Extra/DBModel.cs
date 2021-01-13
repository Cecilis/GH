//namespace GH.Models_Extra
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;

//    public partial class GHDBModel : DbContext
//    {
//        public GHDBModel()
//            : base("name=GHConnection")
//        {
//        }

//        public virtual DbSet<AlertasMantenimientos> AlertasMantenimientos { get; set; }
//        public virtual DbSet<Bases> Bases { get; set; }
//        public virtual DbSet<Categorias> Categorias { get; set; }
//        public virtual DbSet<Causas> Causas { get; set; }
//        public virtual DbSet<Centros> Centros { get; set; }
//        public virtual DbSet<Certificados> Certificados { get; set; }
//        public virtual DbSet<Cierres> Cierres { get; set; }
//        public virtual DbSet<Clientes> Clientes { get; set; }
//        public virtual DbSet<ClientesZonas> ClientesZonas { get; set; }
//        public virtual DbSet<Combustible> Combustible { get; set; }
//        public virtual DbSet<Contrato> Contrato { get; set; }
//        public virtual DbSet<ContratoPersonas> ContratoPersonas { get; set; }
//        public virtual DbSet<ContratoUnidades> ContratoUnidades { get; set; }
//        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
//        public virtual DbSet<CotizacionDetalles> CotizacionDetalles { get; set; }
//        public virtual DbSet<Destinos> Destinos { get; set; }
//        public virtual DbSet<DocumentosPersona> DocumentosPersona { get; set; }
//        public virtual DbSet<DocumentosUnidad> DocumentosUnidad { get; set; }
//        public virtual DbSet<EasyTrack> EasyTrack { get; set; }
//        public virtual DbSet<Empleados> Empleados { get; set; }
//        public virtual DbSet<Estados> Estados { get; set; }
//        public virtual DbSet<EstadosMantenimiento> EstadosMantenimiento { get; set; }
//        public virtual DbSet<Gremios> Gremios { get; set; }
//        public virtual DbSet<Horas> Horas { get; set; }
//        public virtual DbSet<HorasCamioneros> HorasCamioneros { get; set; }
//        public virtual DbSet<Kilometraje> Kilometraje { get; set; }
//        public virtual DbSet<Lugares> Lugares { get; set; }
//        public virtual DbSet<Mantenimientos> Mantenimientos { get; set; }
//        public virtual DbSet<MantenimientosEstados> MantenimientosEstados { get; set; }
//        public virtual DbSet<MantenimientosProgramados> MantenimientosProgramados { get; set; }
//        public virtual DbSet<MantenimientosRepuestos> MantenimientosRepuestos { get; set; }
//        public virtual DbSet<MantenimientosTareas> MantenimientosTareas { get; set; }
//        public virtual DbSet<Notas> Notas { get; set; }
//        public virtual DbSet<NotasDetalles> NotasDetalles { get; set; }
//        public virtual DbSet<NotasEstados> NotasEstados { get; set; }
//        public virtual DbSet<OC> OC { get; set; }
//        public virtual DbSet<OCDetalles> OCDetalles { get; set; }
//        public virtual DbSet<Parametros> Parametros { get; set; }
//        public virtual DbSet<Personas> Personas { get; set; }
//        public virtual DbSet<PMP> PMP { get; set; }
//        public virtual DbSet<PMPDetalle> PMPDetalle { get; set; }
//        public virtual DbSet<Presupuestos> Presupuestos { get; set; }
//        public virtual DbSet<Programas> Programas { get; set; }
//        public virtual DbSet<Proveedores> Proveedores { get; set; }
//        public virtual DbSet<Provincias> Provincias { get; set; }
//        public virtual DbSet<Remitos> Remitos { get; set; }
//        public virtual DbSet<Repuestos> Repuestos { get; set; }
//        public virtual DbSet<RTO> RTO { get; set; }
//        public virtual DbSet<Sectores> Sectores { get; set; }
//        public virtual DbSet<ServicioItems> ServicioItems { get; set; }
//        public virtual DbSet<Servicios> Servicios { get; set; }
//        public virtual DbSet<SubTipoUnidad> SubTipoUnidad { get; set; }
//        public virtual DbSet<SupervisorEmpleados> SupervisorEmpleados { get; set; }
//        public virtual DbSet<Tanques> Tanques { get; set; }
//        public virtual DbSet<Tareas> Tareas { get; set; }
//        public virtual DbSet<TipoCertificado> TipoCertificado { get; set; }
//        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
//        public virtual DbSet<TipoPersona> TipoPersona { get; set; }
//        public virtual DbSet<TipoServicio> TipoServicio { get; set; }
//        public virtual DbSet<TipoTarea> TipoTarea { get; set; }
//        public virtual DbSet<TipoUnidad> TipoUnidad { get; set; }
//        public virtual DbSet<Unidades> Unidades { get; set; }
//        public virtual DbSet<Usuarios> Usuarios { get; set; }
//        public virtual DbSet<UsuariosEmpleados> UsuariosEmpleados { get; set; }
//        public virtual DbSet<Vehiculos> Vehiculos { get; set; }
//        public virtual DbSet<Zonas> Zonas { get; set; }
//        public virtual DbSet<ClientesLugares> ClientesLugares { get; set; }
//        public virtual DbSet<EmpleadosCentros> EmpleadosCentros { get; set; }
//        public virtual DbSet<KMET> KMET { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<AlertasMantenimientos>()
//                .Property(e => e.ultimo_valor)
//                .IsUnicode(false);

//            modelBuilder.Entity<AlertasMantenimientos>()
//                .Property(e => e.valor_actual)
//                .IsUnicode(false);

//            modelBuilder.Entity<Bases>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Bases>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Bases>()
//                .HasMany(e => e.Empleados)
//                .WithMany(e => e.Bases)
//                .Map(m => m.ToTable("EmpleadosBases").MapLeftKey("id_base").MapRightKey("id_empleado"));

//            modelBuilder.Entity<Categorias>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Causas>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Causas>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Centros>()
//                .Property(e => e.centro)
//                .IsUnicode(false);

//            modelBuilder.Entity<Certificados>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .Property(e => e.Denominacion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .Property(e => e.Direccion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .Property(e => e.Telefono)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .Property(e => e.Mail)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .Property(e => e.Localidad)
//                .IsUnicode(false);

//            modelBuilder.Entity<Clientes>()
//                .HasMany(e => e.ClientesLugares)
//                .WithRequired(e => e.Clientes)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Clientes>()
//                .HasMany(e => e.ClientesZonas)
//                .WithRequired(e => e.Clientes)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Clientes>()
//                .HasMany(e => e.Cotizacion)
//                .WithRequired(e => e.Clientes)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Clientes>()
//                .HasMany(e => e.Sectores)
//                .WithMany(e => e.Clientes)
//                .Map(m => m.ToTable("ClientesSectores").MapLeftKey("id_cliente").MapRightKey("id_sector"));

//            modelBuilder.Entity<Clientes>()
//                .HasMany(e => e.Servicios);

//            modelBuilder.Entity<Combustible>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Contrato>()
//                .Property(e => e.cuit_facturacion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Contrato>()
//                .HasMany(e => e.ContratoPersonas)
//                .WithRequired(e => e.Contrato)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Contrato>()
//                .HasMany(e => e.ContratoUnidades)
//                .WithRequired(e => e.Contrato)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.observacion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.estatus)
//                .IsFixedLength()
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.locacion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.contacto)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.telefono)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.tipo_servicio_descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .Property(e => e.servicio_descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Cotizacion>()
//                .HasOptional(e => e.CotizacionDetalles);

//            modelBuilder.Entity<CotizacionDetalles>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<CotizacionDetalles>()
//                .Property(e => e.precio)
//                .HasPrecision(19, 4);

//            modelBuilder.Entity<Destinos>()
//                .Property(e => e.destino)
//                .IsUnicode(false);

//            modelBuilder.Entity<Destinos>()
//                .HasMany(e => e.Notas)
//                .WithRequired(e => e.Destinos)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Destinos>()
//                .HasMany(e => e.Empleados)
//                .WithMany(e => e.Destinos)
//                .Map(m => m.ToTable("EmpleadosDestinos").MapLeftKey("id_destino").MapRightKey("id_empleado"));

//            modelBuilder.Entity<DocumentosPersona>()
//                .Property(e => e.ruta)
//                .IsUnicode(false);

//            modelBuilder.Entity<DocumentosUnidad>()
//                .Property(e => e.ruta)
//                .IsUnicode(false);

//            modelBuilder.Entity<EasyTrack>()
//                .Property(e => e.patente)
//                .IsUnicode(false);

//            modelBuilder.Entity<EasyTrack>()
//                .Property(e => e.horas_uso)
//                .IsUnicode(false);

//            modelBuilder.Entity<EasyTrack>()
//                .Property(e => e.fecha)
//                .IsFixedLength();

//            modelBuilder.Entity<Empleados>()
//                .Property(e => e.Apellido)
//                .IsUnicode(false);

//            modelBuilder.Entity<Empleados>()
//                .Property(e => e.Nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Empleados>()
//                .Property(e => e.Mail)
//                .IsUnicode(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.EmpleadosCentros)
//                .WithRequired(e => e.Empleados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.Horas)
//                .WithRequired(e => e.Empleados)
//                .HasForeignKey(e => e.id_empleado)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.Horas1)
//                .WithOptional(e => e.Empleados1)
//                .HasForeignKey(e => e.id_coequiper1);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.Horas2)
//                .WithOptional(e => e.Empleados2)
//                .HasForeignKey(e => e.id_coequiper2);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.Horas3)
//                .WithOptional(e => e.Empleados3)
//                .HasForeignKey(e => e.id_coequiper3);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.HorasCamioneros)
//                .WithRequired(e => e.Empleados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.Remitos)
//                .WithRequired(e => e.Empleados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.SupervisorEmpleados)
//                .WithRequired(e => e.Empleados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Empleados>()
//                .HasMany(e => e.UsuariosEmpleados)
//                .WithRequired(e => e.Empleados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Estados>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Estados>()
//                .HasMany(e => e.AlertasMantenimientos)
//                .WithRequired(e => e.Estados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Estados>()
//                .HasMany(e => e.MantenimientosEstados)
//                .WithRequired(e => e.Estados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Estados>()
//                .HasMany(e => e.NotasEstados)
//                .WithRequired(e => e.Estados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<EstadosMantenimiento>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Gremios>()
//                .Property(e => e.gremio)
//                .IsUnicode(false);

//            modelBuilder.Entity<Horas>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Horas>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Horas>()
//                .Property(e => e.accion)
//                .IsUnicode(false);

//            modelBuilder.Entity<HorasCamioneros>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<HorasCamioneros>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<HorasCamioneros>()
//                .Property(e => e.accion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Kilometraje>()
//                .Property(e => e.usuario_carga)
//                .IsUnicode(false);

//            modelBuilder.Entity<Lugares>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Lugares>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Lugares>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Lugares>()
//                .Property(e => e.accion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Lugares>()
//                .HasMany(e => e.Horas)
//                .WithOptional(e => e.Lugares)
//                .HasForeignKey(e => e.id_lugar);

//            modelBuilder.Entity<Lugares>()
//                .HasMany(e => e.Horas1)
//                .WithOptional(e => e.Lugares1)
//                .HasForeignKey(e => e.viaja_desde);

//            modelBuilder.Entity<Lugares>()
//                .HasMany(e => e.Horas2)
//                .WithOptional(e => e.Lugares2)
//                .HasForeignKey(e => e.viaja_hasta);

//            modelBuilder.Entity<Lugares>()
//                .HasMany(e => e.HorasCamioneros)
//                .WithOptional(e => e.Lugares)
//                .HasForeignKey(e => e.id_zona);

//            modelBuilder.Entity<Mantenimientos>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Mantenimientos>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Mantenimientos>()
//                .HasMany(e => e.MantenimientosEstados)
//                .WithRequired(e => e.Mantenimientos)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Mantenimientos>()
//                .HasMany(e => e.MantenimientosRepuestos)
//                .WithRequired(e => e.Mantenimientos)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Mantenimientos>()
//                .HasMany(e => e.MantenimientosTareas)
//                .WithRequired(e => e.Mantenimientos)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<MantenimientosEstados>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<MantenimientosEstados>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<MantenimientosProgramados>()
//                .Property(e => e.valor_realizado)
//                .IsUnicode(false);

//            modelBuilder.Entity<MantenimientosProgramados>()
//                .Property(e => e.valor_planificado)
//                .IsUnicode(false);

//            modelBuilder.Entity<MantenimientosProgramados>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<MantenimientosTareas>()
//                .Property(e => e.comentarios)
//                .IsUnicode(false);

//            modelBuilder.Entity<Notas>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Notas>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Notas>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Notas>()
//                .HasMany(e => e.NotasDetalles)
//                .WithRequired(e => e.Notas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Notas>()
//                .HasMany(e => e.Presupuestos)
//                .WithRequired(e => e.Notas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<NotasDetalles>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<NotasDetalles>()
//                .Property(e => e.proveedor_sugerido)
//                .IsUnicode(false);

//            modelBuilder.Entity<NotasEstados>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<NotasEstados>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<NotasEstados>()
//                .HasMany(e => e.Notas)
//                .WithRequired(e => e.NotasEstados)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<OC>()
//                .Property(e => e.usuario_aprobador)
//                .IsUnicode(false);

//            modelBuilder.Entity<OC>()
//                .Property(e => e.usuario_confecciona)
//                .IsUnicode(false);

//            modelBuilder.Entity<OC>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<OCDetalles>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Parametros>()
//                .Property(e => e.parametro)
//                .IsUnicode(false);

//            modelBuilder.Entity<Parametros>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Personas>()
//                .Property(e => e.Apellido)
//                .IsUnicode(false);

//            modelBuilder.Entity<Personas>()
//                .Property(e => e.Nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Personas>()
//                .Property(e => e.Mail)
//                .IsUnicode(false);

//            modelBuilder.Entity<Personas>()
//                .HasMany(e => e.ContratoUnidades)
//                .WithRequired(e => e.Personas)
//                .HasForeignKey(e => e.id_unidad)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Personas>()
//                .HasMany(e => e.DocumentosPersona)
//                .WithRequired(e => e.Personas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<PMP>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<PMP>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<PMP>()
//                .HasMany(e => e.PMPDetalle)
//                .WithRequired(e => e.PMP)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Presupuestos>()
//                .Property(e => e.total)
//                .HasPrecision(8, 2);

//            modelBuilder.Entity<Presupuestos>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Presupuestos>()
//                .Property(e => e.archivo)
//                .IsUnicode(false);

//            modelBuilder.Entity<Programas>()
//                .Property(e => e.valor_inicial)
//                .IsUnicode(false);

//            modelBuilder.Entity<Programas>()
//                .Property(e => e.valor_actual)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.cuit)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.razon_social)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.nombre_comercial)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.domicilio)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.localidad)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.telefono)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.contacto)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.mail)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .Property(e => e.web)
//                .IsUnicode(false);

//            modelBuilder.Entity<Proveedores>()
//                .HasMany(e => e.Presupuestos)
//                .WithRequired(e => e.Proveedores)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Provincias>()
//                .Property(e => e.provincia)
//                .IsUnicode(false);

//            modelBuilder.Entity<Remitos>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Repuestos>()
//                .Property(e => e.repuesto)
//                .IsUnicode(false);

//            modelBuilder.Entity<Repuestos>()
//                .Property(e => e.identificador)
//                .IsUnicode(false);

//            modelBuilder.Entity<Repuestos>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Repuestos>()
//                .HasMany(e => e.MantenimientosRepuestos)
//                .WithRequired(e => e.Repuestos)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<RTO>()
//                .Property(e => e.planta)
//                .IsUnicode(false);

//            modelBuilder.Entity<RTO>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Sectores>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Sectores>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<ServicioItems>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<ServicioItems>()
//                .Property(e => e.precio)
//                .HasPrecision(19, 4);

//            modelBuilder.Entity<Servicios>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Servicios>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Servicios>()
//                .HasMany(e => e.TiposServicio)
//                .WithRequired(e => e.Servicios)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<SubTipoUnidad>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<SubTipoUnidad>()
//                .HasMany(e => e.Unidades)
//                .WithOptional(e => e.SubTipoUnidad)
//                .HasForeignKey(e => e.subtipo_unidad);

//            modelBuilder.Entity<Tanques>()
//                .Property(e => e.tanque)
//                .IsUnicode(false);

//            modelBuilder.Entity<Tanques>()
//                .Property(e => e.r)
//                .HasPrecision(5, 2);

//            modelBuilder.Entity<Tanques>()
//                .Property(e => e.L)
//                .HasPrecision(5, 2);

//            modelBuilder.Entity<Tanques>()
//                .Property(e => e.h)
//                .HasPrecision(5, 2);

//            modelBuilder.Entity<Tanques>()
//                .HasMany(e => e.Combustible)
//                .WithRequired(e => e.Tanques)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Tareas>()
//                .Property(e => e.tarea)
//                .IsUnicode(false);

//            modelBuilder.Entity<Tareas>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<Tareas>()
//                .HasMany(e => e.AlertasMantenimientos)
//                .WithRequired(e => e.Tareas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Tareas>()
//                .HasMany(e => e.MantenimientosProgramados)
//                .WithRequired(e => e.Tareas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Tareas>()
//                .HasMany(e => e.MantenimientosTareas)
//                .WithRequired(e => e.Tareas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Tareas>()
//                .HasMany(e => e.Programas)
//                .WithRequired(e => e.Tareas)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<TipoCertificado>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoCertificado>()
//                .HasMany(e => e.Certificados)
//                .WithRequired(e => e.TipoCertificado)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<TipoDocumento>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoDocumento>()
//                .Property(e => e.aplica_a)
//                .IsFixedLength()
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoDocumento>()
//                .HasMany(e => e.DocumentosPersona)
//                .WithRequired(e => e.TipoDocumento)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<TipoDocumento>()
//                .HasMany(e => e.DocumentosUnidad)
//                .WithRequired(e => e.TipoDocumento)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<TipoPersona>()
//                .Property(e => e.descripc)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoPersona>()
//                .HasMany(e => e.Personas)
//                .WithRequired(e => e.TipoPersona)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<TipoServicio>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoTarea>()
//                .Property(e => e.tipo_tarea)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoUnidad>()
//                .Property(e => e.descripcion)
//                .IsUnicode(false);

//            modelBuilder.Entity<TipoUnidad>()
//                .HasMany(e => e.SubTipoUnidad)
//                .WithRequired(e => e.TipoUnidad)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .Property(e => e.Patente)
//                .IsUnicode(false);

//            modelBuilder.Entity<Unidades>()
//                .Property(e => e.Marca)
//                .IsUnicode(false);

//            modelBuilder.Entity<Unidades>()
//                .Property(e => e.Modelo)
//                .IsUnicode(false);

//            modelBuilder.Entity<Unidades>()
//                .Property(e => e.Chasis)
//                .IsUnicode(false);

//            modelBuilder.Entity<Unidades>()
//                .Property(e => e.Observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.AlertasMantenimientos)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.Certificados)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.Combustible)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.ContratoUnidades)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.DocumentosUnidad)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.Kilometraje)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.Mantenimientos)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.MantenimientosProgramados)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Unidades>()
//                .HasMany(e => e.Programas)
//                .WithRequired(e => e.Unidades)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<Usuarios>()
//                .Property(e => e.Apellido)
//                .IsUnicode(false);

//            modelBuilder.Entity<Usuarios>()
//                .Property(e => e.Nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Usuarios>()
//                .Property(e => e.Mail)
//                .IsUnicode(false);

//            modelBuilder.Entity<UsuariosEmpleados>()
//                .Property(e => e.usuario)
//                .IsUnicode(false);

//            modelBuilder.Entity<Vehiculos>()
//                .Property(e => e.Patente)
//                .IsUnicode(false);

//            modelBuilder.Entity<Vehiculos>()
//                .Property(e => e.Marca)
//                .IsUnicode(false);

//            modelBuilder.Entity<Vehiculos>()
//                .Property(e => e.Modelo)
//                .IsUnicode(false);

//            modelBuilder.Entity<Vehiculos>()
//                .Property(e => e.Observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<Vehiculos>()
//                .HasMany(e => e.HorasCamioneros)
//                .WithOptional(e => e.Vehiculos)
//                .HasForeignKey(e => e.id_tractor);

//            modelBuilder.Entity<Zonas>()
//                .Property(e => e.nombre)
//                .IsUnicode(false);

//            modelBuilder.Entity<Zonas>()
//                .Property(e => e.observaciones)
//                .IsUnicode(false);

//            modelBuilder.Entity<KMET>()
//                .Property(e => e.Dominio)
//                .IsUnicode(false);

//            modelBuilder.Entity<KMET>()
//                .Property(e => e.Kilometros)
//                .IsUnicode(false);
//        }
//    }
//}
