namespace SWRCVA.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<CategoriaMat> CategoriaMat { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ColorMat> ColorMat { get; set; }
        public virtual DbSet<ColorMaterial> ColorMaterial { get; set; }
        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<ListaMatProducto> ListaMatProducto { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<ProductoCotizacion> ProductoCotizacion { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<SubCategoria> SubCategoria { get; set; }
        public virtual DbSet<TipoMaterial> TipoMaterial { get; set; }
        public virtual DbSet<TipoProducto> TipoProducto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Valor> Valor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoriaMat>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<CategoriaMat>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<CategoriaMat>()
                .HasMany(e => e.ColorMat)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMaterial)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoriaMat>()
                .HasMany(e => e.Material)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoriaMat>()
                .HasMany(e => e.SubCategoria)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoriaMat>()
                .HasMany(e => e.TipoMaterial)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMat)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Cliente>()
                 .Property(e => e.Usuario)
                 .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Cotizacion)
                .WithRequired(e => e.Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Factura)
                .WithRequired(e => e.Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ColorMat>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<ColorMat>()
                .HasMany(e => e.ColorMaterial)
                .WithRequired(e => e.ColorMat)
                .HasForeignKey(e => e.IdColorMat)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ColorMat>()
            .HasMany(e => e.ProductoCotizacion)
            .WithRequired(e => e.ColorMat)
            .HasForeignKey(e => e.IdColorVidrio)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<ColorMat>()
                .HasMany(e => e.ProductoCotizacion1)
                .WithRequired(e => e.ColorMat1)
                .HasForeignKey(e => e.IdColorAluminio)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<ColorMaterial>()
                .Property(e => e.Costo)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Cotizacion>()
                .Property(e => e.Estado)
                .IsFixedLength();

            modelBuilder.Entity<Cotizacion>()
                .Property(e => e.MontoParcial)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Cotizacion>()
                .HasMany(e => e.ProductoCotizacion)
                .WithRequired(e => e.Cotizacion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetalleFactura>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<DetalleFactura>()
                .Property(e => e.MontoParcial)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Factura>()
                .Property(e => e.MontoTotal)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Factura>()
                .Property(e => e.MontoPagar)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Factura>()
                .HasMany(e => e.DetalleFactura)
                .WithRequired(e => e.Factura)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Costo)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ColorMaterial)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ListaMatProducto)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ProductoCotizacion)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.Usuario)
                .IsUnicode(false);
            modelBuilder.Entity<Producto>()
               .HasMany(e => e.DetalleFactura)
               .WithRequired(e => e.Producto)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<Producto>()
                .HasMany(e => e.ListaMatProducto)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.ProductoCotizacion)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductoCotizacion>()
                .Property(e => e.CantMaterial)
                .HasPrecision(12, 2);
            modelBuilder.Entity<ProductoCotizacion>()
                .Property(e => e.Instalacion)
                .HasPrecision(12, 2);

            modelBuilder.Entity<ProductoCotizacion>()
                .Property(e => e.Ancho)
                .HasPrecision(12, 2);

            modelBuilder.Entity<ProductoCotizacion>()
                .Property(e => e.Alto)
                .HasPrecision(12, 2);
            modelBuilder.Entity<Proveedor>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Proveedor>()
                .HasMany(e => e.Material)
                .WithRequired(e => e.Proveedor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuario1)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubCategoria>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<TipoMaterial>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<TipoProducto>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<TipoProducto>()
                .HasMany(e => e.Producto)
                .WithRequired(e => e.TipoProducto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Usuario1)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Factura)
                .WithRequired(e => e.Usuario1)
                .HasForeignKey(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Valor>()
                .Property(e => e.Porcentaje)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Valor>()
                .Property(e => e.Usuario)
                .IsUnicode(false);
        }
    }
}
