namespace GH.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class GHDBContext : DbContext
    {
        public GHDBContext()
            : base("GHConnection")
        {
            //this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;                       
        }

        public virtual DbSet<Bases> Bases { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Consecutivo> Consecutivo { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<CotizacionDetalles> CotizacionDetalles { get; set; }
        public virtual DbSet<Documentacion> Documentacion { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<Fechas> Fechas { get; set; }
        public virtual DbSet<Gremios> Gremios { get; set; }
        public virtual DbSet<Localidad> Localidad { get; set; }
        public virtual DbSet<Personas> Personas { get; set; }
        public virtual DbSet<Provincias> Provincias { get; set; }
        public virtual DbSet<ItemsServicio> ItemsServicio { get; set; }
        public virtual DbSet<Servicios> Servicios { get; set; }
        public virtual DbSet<SubTipoUnidad> SubTipoUnidad { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoPersona> TipoPersonas { get; set; }
        public virtual DbSet<TipoServicio> TipoServicio { get; set; }
        public virtual DbSet<TipoUnidad> TipoUnidad { get; set; }
        public virtual DbSet<Unidades> Unidades { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            //modelBuilder.Entity<Bases>()
            //    .HasMany(e => e.Empleados)
            //    .WithRequired(e => e.Bases)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Clientes>()
            //    .HasMany(e => e.Cotizacion)
            //    .WithRequired(e => e.Clientes)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Contrato>()
            //    .HasMany(e => e.Personas)
            //    .WithMany(e => e.Contratos)
            //    .Map(m => m.ToTable("ContratosPersonas"));

            //modelBuilder.Entity<Contrato>()
            //    .HasMany(e => e.Unidades)
            //    .WithMany(e => e.Contratos)
            //    .Map(m => m.ToTable("UnidadesContrato"));

            //modelBuilder.Entity<Cotizacion>()
            //    .Property(e => e.total_impuesto)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<Cotizacion>()
            //    .Property(e => e.subtotal)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<Cotizacion>()
            //    .Property(e => e.total)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<Cotizacion>()
            //    .HasOptional(e => e.Contrato)
            //    .WithRequired(e => e.Cotizacion);

            //modelBuilder.Entity<CotizacionDetalles>()
            //    .Property(e => e.precio_item_servicio)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<CotizacionDetalles>()
            //    .Property(e => e.precio)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<CotizacionDetalles>()
            //    .Property(e => e.total_impuesto)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<CotizacionDetalles>()
            //    .Property(e => e.subtotal)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<CotizacionDetalles>()
            //    .Property(e => e.total)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<Documentacion>()
            //    .HasMany(e => e.Personas)
            //    .WithMany(e => e.Documentacion)
            //    .Map(m => m.ToTable("DocumentacionPersonas"));

            //modelBuilder.Entity<Documentacion>()
            //    .HasMany(e => e.Unidades)
            //    .WithMany(e => e.Documentacion)
            //    .Map(m => m.ToTable("DocumentacionUnidades"));

            //modelBuilder.Entity<Empleados>()
            //    .HasMany(e => e.Personas)
            //    .WithRequired(e => e.Empleado)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Gremios>()
            //    .HasMany(e => e.Empleados)
            //    .WithRequired(e => e.Gremios)
            //    .HasForeignKey(e => e.Tipo_Empleado)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Gremios>()
            //    .HasMany(e => e.Personas)
            //    .WithRequired(e => e.Gremio)
            //    .HasForeignKey(e => e.id_tipo_empleado)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<ItemsServicio>()
            //    .Property(e => e.precio)
            //    .HasPrecision(19, 4);

            //modelBuilder.Entity<ItemsServicio>()
            //    .HasMany(e => e.CotizacionDetalles)
            //    .WithRequired(e => e.ItemsServicio)
            //    .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<Localidad>()
            ////    .HasMany(e => e.Clientes)
            ////    .WithRequired(e => e.Localidad)
            ////    .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<Localidad>()
            ////    .HasMany(e => e.Cotizacion)
            ////    .WithRequired(e => e.Localidad)
            ////    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Provincias>()
            //    .HasMany(e => e.Localidades)
            //    .WithRequired(e => e.Provincias)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Servicios>()
            //    .HasMany(e => e.TiposServicio)
            //    .WithRequired(e => e.Servicios)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<SubTipoUnidad>()
            //    .HasMany(e => e.Unidades)
            //    .WithRequired(e => e.SubTipoUnidad)
            //    .HasForeignKey(e => e.subtipo_unidad)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TipoDocumento>()
            //    .Property(e => e.aplica_a)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<TipoDocumento>()
            //    .HasMany(e => e.Documentacion)
            //    .WithRequired(e => e.TipoDocumento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TipoPersona>()
            //    .HasMany(e => e.Personas)
            //    .WithRequired(e => e.TipoPersona)
            //    .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<TipoServicio>()
            ////    .HasMany(e => e.Cotizaciones)
            ////    .WithRequired(e => e.TipoServicio)
            ////    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TipoServicio>()
            //    .HasMany(e => e.ItemsServicio)
            //    .WithRequired(e => e.TipoServicio)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TipoUnidad>()
            //    .HasMany(e => e.SubTipoUnidad)
            //    .WithRequired(e => e.TipoUnidad)
            //    .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<UnidadMedida>()
            ////    .HasMany(e => e.CotizacionDetalles)
            ////    .WithRequired(e => e.UnidadMedida)
            ////    .WillCascadeOnDelete(false);

            ////modelBuilder.Entity<UnidadMedida>()
            ////    .HasMany(e => e.ItemsServicio)
            ////    .WithRequired(e => e.UnidadMedida)
            ////    .WillCascadeOnDelete(false);
        }

    }
}