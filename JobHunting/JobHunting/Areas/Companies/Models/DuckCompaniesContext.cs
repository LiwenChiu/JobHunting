﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Companies.Models;

public partial class DuckCompaniesContext : DbContext
{
    public DuckCompaniesContext(DbContextOptions<DuckCompaniesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971C4CEDC7988B");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Benefits).HasMaxLength(200);
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.ContactEmail)
                .IsRequired()
                .HasMaxLength(320);
            entity.Property(e => e.ContactName)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.ContactPhone)
                .IsRequired()
                .HasMaxLength(24);
            entity.Property(e => e.Guinumber).HasColumnName("GUINumber");
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(16);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}