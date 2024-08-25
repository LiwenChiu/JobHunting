﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Models;

public partial class DuckContext : DbContext
{
    public DuckContext(DbContextOptions<DuckContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateLetter> CandidateLetters { get; set; }

    public virtual DbSet<CandidateOpeningRecord> CandidateOpeningRecords { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyApproval> CompanyApprovals { get; set; }

    public virtual DbSet<CompanyLetter> CompanyLetters { get; set; }

    public virtual DbSet<CompanyOrder> CompanyOrders { get; set; }

    public virtual DbSet<CompanyResumeRecord> CompanyResumeRecords { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Opening> Openings { get; set; }

    public virtual DbSet<OpeningTag> OpeningTags { get; set; }

    public virtual DbSet<PricingPlan> PricingPlans { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<ResumeTag> ResumeTags { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagClass> TagClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E8FF408F7F");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
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

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539BFCFC5B39BB");

            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
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
                .IsFixedLength()
                .HasColumnName("NationalID");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.Phone).HasMaxLength(24);
        });

        modelBuilder.Entity<CandidateLetter>(entity =>
        {
            entity.HasKey(e => e.LetterId).HasName("PK__Candidat__AE46E81176A3F6B3");

            entity.Property(e => e.LetterId).HasColumnName("LetterID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Class)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.SubjectLine)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Admin).WithMany(p => p.CandidateLetters)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Candidate__Admin__628FA481");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateLetters)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Candidate__Candi__619B8048");
        });

        modelBuilder.Entity<CandidateOpeningRecord>(entity =>
        {
            entity.HasKey(e => e.CandidateOpeningRecordId).HasName("PK__Candidat__525E147ADA35A0CE");

            entity.Property(e => e.CandidateOpeningRecordId).HasColumnName("CandidateOpeningRecordID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.HireYn).HasColumnName("HireYN");
            entity.Property(e => e.InterviewYn).HasColumnName("InterviewYN");
            entity.Property(e => e.LikeYn).HasColumnName("LikeYN");
            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.OpeningTitle)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateOpeningRecords)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Candidate__Candi__2F10007B");

            entity.HasOne(d => d.Opening).WithMany(p => p.CandidateOpeningRecords)
                .HasForeignKey(d => d.OpeningId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Candidate__Openi__300424B4");
        });

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

        modelBuilder.Entity<CompanyApproval>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.AdminId }).HasName("PK__CompanyA__BA8EE2023969FF22");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Admin).WithMany(p => p.CompanyApprovals)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompanyAp__Admin__59063A47");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyApprovals)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyAp__Compa__571DF1D5");
        });

        modelBuilder.Entity<CompanyLetter>(entity =>
        {
            entity.HasKey(e => e.LetterId).HasName("PK__CompanyL__AE46E81163AD1351");

            entity.Property(e => e.LetterId).HasColumnName("LetterID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Class)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.SubjectLine)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Admin).WithMany(p => p.CompanyLetters)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyLe__Admin__5DCAEF64");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyLetters)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyLe__Compa__5CD6CB2B");
        });

        modelBuilder.Entity<CompanyOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__CompanyO__C3905BAFA57C1706");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);
            entity.Property(e => e.Guinumber).HasColumnName("GUINumber");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.PlanTitle)
                .IsRequired()
                .HasMaxLength(40);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__Compa__4D94879B");

            entity.HasOne(d => d.Plan).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__PlanI__4E88ABD4");
        });

        modelBuilder.Entity<CompanyResumeRecord>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.ResumeId }).HasName("PK__CompanyR__40EA667DBD5A8BBE");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.HireYn).HasColumnName("HireYN");
            entity.Property(e => e.InterviewYn).HasColumnName("InterviewYN");
            entity.Property(e => e.LikeYn).HasColumnName("LikeYN");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyRe__Compa__35BCFE0A");

            entity.HasOne(d => d.Resume).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK__CompanyRe__Resum__36B12243");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32BDEA99AF");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.AppointmentTime).HasColumnType("datetime");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.SubjectLine)
                .IsRequired()
                .HasMaxLength(60);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Candi__52593CB8");

            entity.HasOne(d => d.Company).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Compa__5165187F");
        });

        modelBuilder.Entity<Opening>(entity =>
        {
            entity.HasKey(e => e.OpeningId).HasName("PK__Openings__808F871398B05998");

            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Benefits).HasMaxLength(200);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactEmail).HasMaxLength(320);
            entity.Property(e => e.ContactName)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.ContactPhone)
                .IsRequired()
                .HasMaxLength(24);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.InterviewYn).HasColumnName("InterviewYN");
            entity.Property(e => e.SalaryMax).HasColumnType("money");
            entity.Property(e => e.SalaryMin).HasColumnType("money");
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.TitleClass).HasMaxLength(100);

            entity.HasOne(d => d.Company).WithMany(p => p.Openings)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Openings__Compan__267ABA7A");
        });

        modelBuilder.Entity<OpeningTag>(entity =>
        {
            entity.HasKey(e => new { e.OpeningId, e.TagId }).HasName("PK__OpeningT__56D848B7AAB55673");

            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Opening).WithMany(p => p.OpeningTags)
                .HasForeignKey(d => d.OpeningId)
                .HasConstraintName("FK__OpeningTa__Openi__3F466844");

            entity.HasOne(d => d.Tag).WithMany(p => p.OpeningTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__OpeningTa__TagID__403A8C7D");
        });

        modelBuilder.Entity<PricingPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PricingP__755C22D76A1BB160");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Discount)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(5, 4)");
            entity.Property(e => e.Intro).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(40);
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.HasKey(e => e.ResumeId).HasName("PK__Resumes__D7D7A3176ADFB873");

            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(60);
            entity.Property(e => e.TitleClass).HasMaxLength(100);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resumes__Candida__2C3393D0");
        });

        modelBuilder.Entity<ResumeTag>(entity =>
        {
            entity.HasKey(e => new { e.ResumeId, e.TagId }).HasName("PK__ResumeTa__01806CB3984F0756");

            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeTags)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK__ResumeTag__Resum__440B1D61");

            entity.HasOne(d => d.Tag).WithMany(p => p.ResumeTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK__ResumeTag__TagID__44FF419A");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CFA4C12ECC654");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.TagClassId).HasColumnName("TagClassID");
            entity.Property(e => e.TagName)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasOne(d => d.TagClass).WithMany(p => p.Tags)
                .HasForeignKey(d => d.TagClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tags__TagClassID__3C69FB99");
        });

        modelBuilder.Entity<TagClass>(entity =>
        {
            entity.HasKey(e => e.TagClassId).HasName("PK__TagClass__94933B0067EE1E83");

            entity.Property(e => e.TagClassId).HasColumnName("TagClassID");
            entity.Property(e => e.TagClass1)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("TagClass");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}