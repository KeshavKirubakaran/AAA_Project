using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AAA_Project.Models;

public partial class AaaDbContext : DbContext
{
    public AaaDbContext()
    {
    }

    public AaaDbContext(DbContextOptions<AaaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AaaCity> AaaCities { get; set; }

    public virtual DbSet<AaaMobileModel> AaaMobileModels { get; set; }

    public virtual DbSet<AaaSalesInfo> AaaSalesInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AaaCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AAA_City__3214EC27269F887D");

            entity.ToTable("AAA_City");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("City_Name");
        });

        modelBuilder.Entity<AaaMobileModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AAA_Mobi__3214EC27846EC677");

            entity.ToTable("AAA_MobileModel");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ModelName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("Model_name");
        });

        modelBuilder.Entity<AaaSalesInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AAA_Sale__3214EC275F04371E");

            entity.ToTable("AAA_SalesInfo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CityId).HasColumnName("CITY_ID");
            entity.Property(e => e.DateOfSales)
                .HasColumnType("date")
                .HasColumnName("DATE_OF_SALES");
            entity.Property(e => e.ModelId).HasColumnName("MODEL_ID");
            entity.Property(e => e.SalesCount).HasColumnName("SALES_COUNT");

            entity.HasOne(d => d.City).WithMany(p => p.AaaSalesInfos)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__AAA_Sales__CITY___3B75D760");

            entity.HasOne(d => d.Model).WithMany(p => p.AaaSalesInfos)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK__AAA_Sales__MODEL__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
