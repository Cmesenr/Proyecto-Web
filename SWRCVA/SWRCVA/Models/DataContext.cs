namespace SWRCVA.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<CategoriaMat> CategoriaMat { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ColorMat> ColorMat { get; set; }
        public virtual DbSet<Cotizacion> Cotizacion { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<ListaMatProducto> ListaMatProducto { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Orden> Orden { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<ProductoCotizacion> ProductoCotizacion { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<SubCategoria> SubCategoria { get; set; }
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
                .HasMany(e => e.Material)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CategoriaMat>()
                .HasMany(e => e.SubCategoria)
                .WithRequired(e => e.CategoriaMat)
                .HasForeignKey(e => e.IdCatMat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Telefono)
                .HasPrecision(8, 0);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Factura)
                .WithRequired(e => e.Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ColorMat>()
                .Property(e => e.Nombre)
                .IsFixedLength();

            modelBuilder.Entity<ColorMat>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<ColorMat>()
                .HasMany(e => e.Material)
                .WithOptional(e => e.ColorMat)
                .HasForeignKey(e => e.IdColorMat);

            modelBuilder.Entity<Cotizacion>()
                .Property(e => e.Estado)
                .IsFixedLength();

            modelBuilder.Entity<Cotizacion>()
                .Property(e => e.MontoParcial)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Cotizacion>()
                .HasMany(e => e.Orden)
                .WithRequired(e => e.Cotizacion)
                .WillCascadeOnDelete(false);

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
                .HasOptional(e => e.DetalleFactura)
                .WithRequired(e => e.Factura);

            modelBuilder.Entity<ListaMatProducto>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.Costo)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ListaMatProducto)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.ProductoCotizacion)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Orden>()
                .HasMany(e => e.DetalleFactura)
                .WithRequired(e => e.Orden)
                .HasForeignKey(e => new { e.IdProducto, e.IdCotizacion })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.ListaMatProducto)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.Orden)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.ProductoCotizacion)
                .WithRequired(e => e.Producto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductoCotizacion>()
                .Property(e => e.CantMaterial)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Proveedor>()
                .Property(e => e.Telefono)
                .HasPrecision(8, 0);

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
                .Property(e => e.Usuario)
                .IsUnicode(false);
        }
    }
}
