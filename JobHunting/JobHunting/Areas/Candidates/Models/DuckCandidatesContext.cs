﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Areas.Candidates.Models;

public partial class DuckCandidatesContext : DbContext
{
    public DuckCandidatesContext(DbContextOptions<DuckCandidatesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminRecord> AdminRecords { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyCategory> CompanyCategories { get; set; }

    public virtual DbSet<CompanyClass> CompanyClasses { get; set; }

    public virtual DbSet<CompanyOrder> CompanyOrders { get; set; }

    public virtual DbSet<CompanyResumeRecord> CompanyResumeRecords { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Opening> Openings { get; set; }

    public virtual DbSet<OpinionLetter> OpinionLetters { get; set; }

    public virtual DbSet<PricingPlan> PricingPlans { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<ResumeOpeningRecord> ResumeOpeningRecords { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagClass> TagClasses { get; set; }

    public virtual DbSet<TitleCategory> TitleCategories { get; set; }

    public virtual DbSet<TitleClass> TitleClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_PRC_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE48876CA165B");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(320);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(16);
        });

        modelBuilder.Entity<AdminRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__AdminRec__FBDF78E975C0EC6B");

            entity.Property(e => e.CRUD)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Task)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539B9C4C4D8661");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Degree).HasMaxLength(30);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(320);
            entity.Property(e => e.EmploymentStatus).HasMaxLength(20);
            entity.Property(e => e.MilitaryService).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.NationalId)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.Phone).HasMaxLength(24);

            entity.HasOne(d => d.TitleClass).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.TitleClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Candidate__Title__3B75D760");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CACA0BCB89C");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Benefits).HasMaxLength(200);
            entity.Property(e => e.CompanyClassId)
                .HasMaxLength(2)
                .IsFixedLength();
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
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.GUINumber)
                .IsRequired()
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(16);

            entity.HasOne(d => d.CompanyClass).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CompanyClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Companies__Compa__29572725");
        });

        modelBuilder.Entity<CompanyCategory>(entity =>
        {
            entity.HasKey(e => e.CompanyCategoryId).HasName("PK__CompanyC__0DD411307EB71BB4");

            entity.Property(e => e.CompanyCategoryId)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CompanyCategoryName)
                .IsRequired()
                .HasMaxLength(40);
        });

        modelBuilder.Entity<CompanyClass>(entity =>
        {
            entity.HasKey(e => e.CompanyClassId).HasName("PK__CompanyC__2EBC463F2E48BFF6");

            entity.Property(e => e.CompanyClassId)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.CompanyCategoryId)
                .IsRequired()
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CompanyClassName)
                .IsRequired()
                .HasMaxLength(40);

            entity.HasOne(d => d.CompanyCategory).WithMany(p => p.CompanyClasses)
                .HasForeignKey(d => d.CompanyCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyCl__Compa__267ABA7A");
        });

        modelBuilder.Entity<CompanyOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CompanyO__C3905BCFADE82DB8");

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.GUINumber)
                .IsRequired()
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PayDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__Compa__6477ECF3");

            entity.HasOne(d => d.Plan).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__PlanI__656C112C");
        });

        modelBuilder.Entity<CompanyResumeRecord>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.ResumeId }).HasName("PK__CompanyR__40EA66A33140012A");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyRe__Compa__4CA06362");

            entity.HasOne(d => d.Resume).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK__CompanyRe__Resum__4D94879B");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E125B529107");

            entity.Property(e => e.AppointmentTime).HasColumnType("datetime");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.SubjectLine)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Candi__6B24EA82");

            entity.HasOne(d => d.Company).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Compa__6A30C649");
        });

        modelBuilder.Entity<Opening>(entity =>
        {
            entity.HasKey(e => e.OpeningId).HasName("PK__Openings__808F87339ABFF481");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Benefits).HasMaxLength(200);
            entity.Property(e => e.ContactEmail).HasMaxLength(320);
            entity.Property(e => e.ContactName)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.ContactPhone)
                .IsRequired()
                .HasMaxLength(24);
            entity.Property(e => e.Degree).HasMaxLength(20);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.SalaryMax).HasColumnType("money");
            entity.Property(e => e.SalaryMin).HasColumnType("money");
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Company).WithMany(p => p.Openings)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Openings__Compan__32E0915F");

            entity.HasMany(d => d.Tags).WithMany(p => p.Openings)
                .UsingEntity<Dictionary<string, object>>(
                    "OpeningTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__OpeningTa__TagId__5812160E"),
                    l => l.HasOne<Opening>().WithMany()
                        .HasForeignKey("OpeningId")
                        .HasConstraintName("FK__OpeningTa__Openi__571DF1D5"),
                    j =>
                    {
                        j.HasKey("OpeningId", "TagId").HasName("PK__OpeningT__56D848A967BC274C");
                        j.ToTable("OpeningTags");
                    });

            entity.HasMany(d => d.TitleClasses).WithMany(p => p.Openings)
                .UsingEntity<Dictionary<string, object>>(
                    "OpeningTitleClass",
                    r => r.HasOne<TitleClass>().WithMany()
                        .HasForeignKey("TitleClassId")
                        .HasConstraintName("FK__OpeningTi__Title__38996AB5"),
                    l => l.HasOne<Opening>().WithMany()
                        .HasForeignKey("OpeningId")
                        .HasConstraintName("FK__OpeningTi__Openi__37A5467C"),
                    j =>
                    {
                        j.HasKey("OpeningId", "TitleClassId").HasName("PK__OpeningT__0743ACD8F319E45D");
                        j.ToTable("OpeningTitleClasses");
                    });
        });

        modelBuilder.Entity<OpinionLetter>(entity =>
        {
            entity.HasKey(e => e.LetterId).HasName("PK__OpinionL__AE46E8F1E86F819C");

            entity.Property(e => e.Class)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.SubjectLine)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Admin).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Admin__71D1E811");

            entity.HasOne(d => d.Candidate).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Candi__70DDC3D8");

            entity.HasOne(d => d.Company).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Compa__6FE99F9F");
        });

        modelBuilder.Entity<PricingPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PricingP__755C22B7E6882515");

            entity.Property(e => e.Discount)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(5, 4)");
            entity.Property(e => e.Intro).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40);
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.HasKey(e => e.ResumeId).HasName("PK__Resumes__D7D7A0F7EC05ADE8");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.ReleaseYN).HasDefaultValue(true);
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__Resumes__Candida__3E52440B");

            entity.HasMany(d => d.Tags).WithMany(p => p.Resumes)
                .UsingEntity<Dictionary<string, object>>(
                    "ResumeTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__ResumeTag__TagId__5BE2A6F2"),
                    l => l.HasOne<Resume>().WithMany()
                        .HasForeignKey("ResumeId")
                        .HasConstraintName("FK__ResumeTag__Resum__5AEE82B9"),
                    j =>
                    {
                        j.HasKey("ResumeId", "TagId").HasName("PK__ResumeTa__01806F6DDD03D432");
                        j.ToTable("ResumeTags");
                    });

            entity.HasMany(d => d.TitleClasses).WithMany(p => p.Resumes)
                .UsingEntity<Dictionary<string, object>>(
                    "ResumeTitleClass",
                    r => r.HasOne<TitleClass>().WithMany()
                        .HasForeignKey("TitleClassId")
                        .HasConstraintName("FK__ResumeTit__Title__4316F928"),
                    l => l.HasOne<Resume>().WithMany()
                        .HasForeignKey("ResumeId")
                        .HasConstraintName("FK__ResumeTit__Resum__4222D4EF"),
                    j =>
                    {
                        j.HasKey("ResumeId", "TitleClassId").HasName("PK__ResumeTi__501B8B1CCECEEDDF");
                        j.ToTable("ResumeTitleClasses");
                    });
        });

        modelBuilder.Entity<ResumeOpeningRecord>(entity =>
        {
            entity.HasKey(e => e.ResumeOpeningRecordId).HasName("PK__ResumeOp__CD5B6F5AE64E1604");

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.OpeningTitle)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Opening).WithMany(p => p.ResumeOpeningRecords)
                .HasForeignKey(d => d.OpeningId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ResumeOpe__Openi__46E78A0C");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeOpeningRecords)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ResumeOpe__Resum__45F365D3");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CF9ACCC45736D");

            entity.Property(e => e.TagClassId).HasDefaultValue(0);
            entity.Property(e => e.TagName)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(d => d.TagClass).WithMany(p => p.Tags)
                .HasForeignKey(d => d.TagClassId)
                .HasConstraintName("FK__Tags__TagClassId__5441852A");
        });

        modelBuilder.Entity<TagClass>(entity =>
        {
            entity.HasKey(e => e.TagClassId).HasName("PK__TagClass__94933B2052EBE830");

            entity.Property(e => e.TagClassName)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<TitleCategory>(entity =>
        {
            entity.HasKey(e => e.TitleCategoryId).HasName("PK__TitleCat__079D34B80FE25684");

            entity.Property(e => e.TitleCategoryName)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<TitleClass>(entity =>
        {
            entity.HasKey(e => e.TitleClassId).HasName("PK__TitleCla__7CC2BEB923443623");

            entity.Property(e => e.TitleCategoryId).HasDefaultValue(0);
            entity.Property(e => e.TitleClassName)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(d => d.TitleCategory).WithMany(p => p.TitleClasses)
                .HasForeignKey(d => d.TitleCategoryId)
                .HasConstraintName("FK__TitleClas__Title__300424B4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}