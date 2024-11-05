﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.Models.Entity;

public partial class KoiCareSystemAtHomeContext : IdentityDbContext<ApplicationUser>
{
    public KoiCareSystemAtHomeContext()
    {
    }

    public KoiCareSystemAtHomeContext(DbContextOptions<KoiCareSystemAtHomeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogImage> BlogImages { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Koi> Kois { get; set; }

    public virtual DbSet<KoiImage> KoiImages { get; set; }

    public virtual DbSet<KoiRecord> KoiRecords { get; set; }

    public virtual DbSet<KoiRemind> KoiReminds { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsImage> NewsImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Pond> Ponds { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Revenue> Revenues { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopRating> ShopRatings { get; set; }

    public virtual DbSet<WaterParameter> WaterParameters { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<VipRecord> VipRecords { get; set; }

    public virtual DbSet<VipPackage> VipPackages { get; set; }

    public virtual DbSet<OrderVipDetail> OrderVipDetails { get; set; }


    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=DELL-NB\\PHATLV;Initial Catalog= KoiCareSystemAtHome;Persist Security Info=True;User ID=sa;Password=12345;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
       //1:1
        modelBuilder.Entity<ApplicationUser>()
       .HasOne(a => a.Shop)
       .WithOne(s => s.User)
       .HasForeignKey<Shop>(s => s.UserId);

        modelBuilder.Entity<ApplicationUser>()
      .HasOne(a => a.Cart)
      .WithOne(s => s.User)
      .HasForeignKey<Cart>(s => s.UserId);

        modelBuilder.Entity<Order>()
            .HasOne(d => d.PaymentTransaction).WithOne(p => p.Order)
                .HasForeignKey<PaymentTransaction>(s => s.VnpOrderInfo);

        //
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E50F8B648A7");

            entity.Property(e => e.BlogId).HasColumnName("BlogId");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId)
               .HasMaxLength(450)
               .HasColumnName("UserId");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blogs__UserID__59063A47");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__BlogComm__C3B4DFCA9EAB0407");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BlogComme__BlogI__628FA481");
            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.BlogComments)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK__BlogComme__UserI__6D0D32F4");
        });

        modelBuilder.Entity<BlogImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Blog_Ima__7516F4ECEE409C37");

            entity.ToTable("Blog_Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageId");
            entity.Property(e => e.BlogId).HasColumnName("BlogId");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogImages)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Blog_Imag__BlogI__68487DD7");
        });


        modelBuilder.Entity<Cart>(e =>
        {
            e.ToTable("Cart");
            e.HasKey(e => e.CartId).HasName("PK__Cart__18103112");
            e.Property(e => e.CartId).HasColumnName("CartId");
            e.Property(e => e.TotalAmount).HasColumnName("TotalAmount");

        });


        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BD3B2F4B4");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
            entity.Property(e => e.Name).HasMaxLength(255);
        });


        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.ProductId }).HasName("PK__Cart__Product__08D097C1B825DECE");

            entity.ToTable("CartItem");

            entity.Property(e => e.CartId).HasColumnName("CartId");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("ProductId");
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.Property(e => e.TotalPrice).HasColumnName("TotalPrice");


            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__CartItem__Cart__5CD6CB2B");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK__CartItems__Product__5DCAEF64");
        });


        modelBuilder.Entity<Koi>(entity =>
        {
            entity.HasKey(e => e.KoiId).HasName("PK__Koi__E03435B8C848C56F");

            entity.ToTable("Koi");

            entity.Property(e => e.KoiId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("KoiId");
            entity.Property(e => e.Color).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Origin).HasMaxLength(255);
            entity.Property(e => e.PondId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("PondId");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Pond).WithMany(p => p.Kois)
                .HasForeignKey(d => d.PondId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Koi__PondID__45F365D3");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.Kois)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK__Koi__UserID__4AB81AF0");
        });

        modelBuilder.Entity<KoiImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Koi_Imag__7516F4ECE51D9912");

            entity.ToTable("Koi_Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageId");
            entity.Property(e => e.KoiId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("KoiId");

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiImages)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Koi_Image__KoiID__4BAC3F29");
        });

        modelBuilder.Entity<KoiRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Koi_Reco__FBDF78C91A7A637C");

            entity.ToTable("Koi_Record");

            entity.Property(e => e.RecordId).HasColumnName("RecordId");
            entity.Property(e => e.KoiId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("KoiId");
            entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiRecords)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Koi_Recor__KoiID__48CFD27E");


            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.KoiRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Koi_Recor__UserI__4E88ABD4");
        });

        modelBuilder.Entity<KoiRemind>(entity =>
        {
            entity.HasKey(e => e.RemindId).HasName("PK__Koi_Remi__C0874AB53A9C5B68");

            entity.ToTable("Koi_Remind");

            entity.Property(e => e.RemindId).HasColumnName("RemindId");
            entity.Property(e => e.DateRemind).HasColumnType("datetime");
            entity.Property(e => e.KoiId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("KoiId");
            entity.Property(e => e.RemindDescription).HasMaxLength(255);

            entity.HasOne(d => d.Koi).WithMany(p => p.KoiReminds)
                .HasForeignKey(d => d.KoiId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Koi_Remin__KoiID__4E88ABD4");
            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.KoiReminds)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK__Koi_Remin__UserI__5535A963");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD35052CA30");

            entity.Property(e => e.NewsId).HasColumnName("NewsId");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__News__Users__6B24EA83");
        });

        modelBuilder.Entity<NewsImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__News_Ima__7516F4ECEE8774FD");

            entity.ToTable("News_Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageId");
            entity.Property(e => e.NewsId).HasColumnName("NewsId");

            entity.HasOne(d => d.News).WithMany(p => p.NewsImages)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__News_Imag__NewsI__6B24EA82");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF8E052D03");

            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.OrderStatus).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.isVipUpgrade).HasDefaultValue(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserId");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.Orders)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK__Orders__UserID__628FA481");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__OrderDet__08D097C1B825DECE");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("ProductId");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__OrderDeta__Order__5CD6CB2B");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK__OrderDeta__Produ__5DCAEF64");
        });

        modelBuilder.Entity<OrderVipDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.VipId }).HasName("PK__OrderDet__08D097C1B825BACA");

            entity.ToTable("OrderVipDetail");

            entity.Property(e => e.OrderId).HasColumnName("OrderId");
            entity.Property(e => e.VipId).HasColumnName("VipId");
            //entity.Property(e => e.CreateDate).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderVipDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__OrderDeta__OrderVip__5CD6CB2B");

            entity.HasOne(d => d.VipPackage).WithMany(p => p.OrderVipDetails)
                .HasForeignKey(d => d.VipId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__VipPackage__OrderVip__5CD6CB2B");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentT__DC31C1F3AA967D38");

            entity.ToTable("PaymentTransaction");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.userId)
        .HasMaxLength(450);

            entity.Property(e => e.VnpTxnRef)
                .HasMaxLength(50);

            entity.Property(e => e.VnpAmount)
                .HasMaxLength(50);

            entity.Property(e => e.VnpBankCode)
                .HasMaxLength(50);

            entity.Property(e => e.VnpBankTranNo)
                .HasMaxLength(50);

            entity.Property(e => e.VnpCardType)
                .HasMaxLength(50);

            entity.Property(e => e.VnpOrderInfo)
                .HasMaxLength(255);

            entity.Property(e => e.VnpPayDate)
                .HasMaxLength(14); // VNPay typically sends date in "yyyyMMddHHmmss" format

            entity.Property(e => e.VnpResponseCode)
                .HasMaxLength(5);

            entity.Property(e => e.VnpTransactionNo)
                .HasMaxLength(50);

            entity.Property(e => e.VnpTransactionStatus)
                .HasMaxLength(5);

            entity.Property(e => e.VnpSecureHash)
                .HasMaxLength(450);

            entity.Property(e => e.VnpTmnCode)
                .HasMaxLength(20);

            entity.Property(e => e.PaymentStatus);

            entity.HasOne(d => d.User).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentTran__AppliUser__5DCAEF70");
        });

        modelBuilder.Entity<Pond>(entity =>
        {
            entity.HasKey(e => e.PondId).HasName("PK__Pond__D18BF854E0A136F2");

            entity.ToTable("Pond");

            entity.Property(e => e.PondId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("PondId");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Thumbnail).IsUnicode(false);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.Ponds)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.Cascade)
              .HasConstraintName("FK__Pond__UserID__398D8EEE");
        });

        modelBuilder.Entity<VipRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VipR__B40CC6ED16BED2A9");

            entity.ToTable("VipRecord");

            entity.HasOne(e => e.User).WithMany(p => p.VipRecords)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VipRecord__User__3C69FB99");

            entity.HasOne(e => e.VipPackage).WithMany(p => p.VipRecords)
                .HasForeignKey(e => e.VipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VipRecord__VipPackage__3C69FB99");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("VipRecordId");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.StartDate).HasDefaultValueSql("GETUTCDATE()").HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");

        });

        modelBuilder.Entity<VipPackage>(entity =>
        {
            entity.HasKey(e => e.VipId).HasName("PK__VipP__B40CC6ED16BED2A9");

            entity.ToTable("VipPackage", t => t.HasCheckConstraint("CK_Options_AllowedValues", "[Options] IN (1, 6, 12)")) ;

            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.Property(e => e.Options)
        .HasColumnName("Options")
        .HasConversion<int>()
        .HasDefaultValue(1);
           


        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED16BED1B0");

            entity.ToTable("Product");

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("ProductId");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__3C69FB99");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
               .HasForeignKey(d => d.ShopId)
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK__Product__Shop__3F466844");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Product___7516F4EC46AA123A");

            entity.ToTable("Product_Image");

            entity.Property(e => e.ImageId).HasColumnName("ImageId");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("ProductId");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Product_I__Produ__3F466844");
        });

        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__Revenue__275F16DD1D3720ED");

            entity.ToTable("Revenue");

            entity.Property(e => e.OrderId).HasColumnName("OrderId");

            entity.HasOne(d => d.Order).WithMany(p => p.Revenues)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Revenue__OrderID__656C112C");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shop__67C556291A1379BC");

            entity.ToTable("Shop");

            entity.Property(e => e.ShopId).HasColumnName("ShopId");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(2, 1)");
            entity.Property(e => e.ShopName).HasMaxLength(255);
            entity.Property(e => e.Thumbnail).IsUnicode(false);
        });

        modelBuilder.Entity<ShopRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__ShopRating__C3905BAF8E052D03");

            entity.Property(e => e.RatingId).HasColumnName("RatingId").IsUnicode(false);
            entity.Property(e => e.Rating).HasDefaultValue(1);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");

            entity.Property(e => e.ShopId).HasColumnName("ShopId");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserId");


            entity.HasOne(d => d.Shop).WithMany(p => p.ShopRatings)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ShopRating__ShopID__59063A47");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.ShopRatings)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK__ShopRating__UserID__628FA481");
        });

        modelBuilder.Entity<WaterParameter>(entity =>
        {
            entity.HasKey(e => e.MeasureId).HasName("PK__Water_Pa__8C56D7606385B788");

            entity.ToTable("Water_Parameter");

            entity.Property(e => e.MeasureId).HasColumnName("MeasureId");
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("GETUTCDATE()").HasColumnType("datetime");
            entity.Property(e => e.PH).HasColumnName("pH");
            entity.Property(e => e.PondId)
                .ValueGeneratedOnAdd()
                .IsUnicode(false)
                .HasColumnName("PondId");

            entity.HasOne(d => d.Pond).WithMany(p => p.WaterParameters)
                .HasForeignKey(d => d.PondId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Water_Par__PondI__5535A963");
            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.WaterParameters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Water_Par__UserI__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
