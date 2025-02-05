using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fShop.Models;

public partial class fShopContext : DbContext
{
    public fShopContext()
    {
    }

    public fShopContext(DbContextOptions<fShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductColor> ProductColors { get; set; }

    public virtual DbSet<ProductSeason> ProductSeasons { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Style> Styles { get; set; }

    public virtual DbSet<TargetAudience> TargetAudiences { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=fShop;User Id=SA;Password=MyStrongPass123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Color__8DA7676DF88375CB");

            entity.ToTable("Color");

            entity.HasIndex(e => e.NameColor, "UQ__Color__A7BD91FFEDAAC9EA").IsUnique();

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.NameColor).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C5061317BFCCA36E");

            entity.ToTable("Material");

            entity.HasIndex(e => e.NameMaterial, "UQ__Material__4CD5CB1513E20451").IsUnique();

            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.NameMaterial).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAFE32F9255");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserID__5CD6CB2B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C9CCFD9F8");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");

            entity.HasOne(d => d.Color).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Color__72C60C4A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__619B8048");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__628FA481");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__SizeI__71D1E811");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDA42A6EAF");

            entity.ToTable("Product");

            entity.HasIndex(e => e.NameProduct, "UQ__Product__DF0C7103F1076C64").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.AudienceId).HasColumnName("AudienceID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.DescriptionProduct).HasMaxLength(500);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.NameProduct).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StyleId).HasColumnName("StyleID");

            entity.HasOne(d => d.Audience).WithMany(p => p.Products)
                .HasForeignKey(d => d.AudienceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Audienc__4BAC3F29");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__49C3F6B7");

            entity.HasOne(d => d.Material).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Materia__4CA06362");

            entity.HasOne(d => d.Style).WithMany(p => p.Products)
                .HasForeignKey(d => d.StyleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__StyleID__4AB81AF0");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2B74A41D9C");

            entity.ToTable("ProductCategory");

            entity.HasIndex(e => e.NameProductCategory, "UQ__ProductC__A8B93719B67D7C03").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.NameProductCategory).HasMaxLength(100);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

            entity.HasOne(d => d.ProductType).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCa__Produ__3B75D760");
        });

        modelBuilder.Entity<ProductColor>(entity =>
        {
            entity.HasKey(e => e.ProductColorId).HasName("PK__ProductC__C5DB681E7475A343");

            entity.ToTable("ProductColor");

            entity.Property(e => e.ProductColorId).HasColumnName("ProductColorID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__Color__693CA210");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__Produ__68487DD7");
        });

        modelBuilder.Entity<ProductSeason>(entity =>
        {
            entity.HasKey(e => e.ProductSeasonId).HasName("PK__ProductS__238D5C55EAC289FF");

            entity.ToTable("ProductSeason");

            entity.Property(e => e.ProductSeasonId).HasColumnName("ProductSeasonID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSeasons)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSe__Produ__52593CB8");

            entity.HasOne(d => d.Season).WithMany(p => p.ProductSeasons)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSe__Seaso__534D60F1");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.ProductSizeId).HasName("PK__ProductS__9DADF571CAE21FFF");

            entity.ToTable("ProductSize");

            entity.Property(e => e.ProductSizeId).HasColumnName("ProductSizeID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__Produ__6FE99F9F");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__SizeI__70DDC3D8");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__ProductT__A1312F4E6302768C");

            entity.ToTable("ProductType");

            entity.HasIndex(e => e.NameProductType, "UQ__ProductT__05FBE73AB46FB4FB").IsUnique();

            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.NameProductType).HasMaxLength(100);
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.SeasonId).HasName("PK__Season__C1814E1824B08764");

            entity.ToTable("Season");

            entity.HasIndex(e => e.NameSeason, "UQ__Season__21828B141D02E1AF").IsUnique();

            entity.Property(e => e.SeasonId).HasColumnName("SeasonID");
            entity.Property(e => e.NameSeason).HasMaxLength(100);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Size__83BD095A11A813ED");

            entity.ToTable("Size");

            entity.HasIndex(e => e.NameSize, "UQ__Size__252E96166A777C49").IsUnique();

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.NameSize).HasMaxLength(50);
        });

        modelBuilder.Entity<Style>(entity =>
        {
            entity.HasKey(e => e.StyleId).HasName("PK__Style__8AD147A0FF2C477E");

            entity.ToTable("Style");

            entity.HasIndex(e => e.NameStyle, "UQ__Style__FAC94E44C2A716B1").IsUnique();

            entity.Property(e => e.StyleId).HasColumnName("StyleID");
            entity.Property(e => e.NameStyle).HasMaxLength(100);
        });

        modelBuilder.Entity<TargetAudience>(entity =>
        {
            entity.HasKey(e => e.AudienceId).HasName("PK__TargetAu__79AE2A46E3496672");

            entity.ToTable("TargetAudience");

            entity.HasIndex(e => e.NameTargetAudience, "UQ__TargetAu__848D8FEBCEA4DDCE").IsUnique();

            entity.Property(e => e.AudienceId).HasColumnName("AudienceID");
            entity.Property(e => e.NameTargetAudience).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC829BECCE");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__85FB4E387B5B5742").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D105346A8C1B6F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
