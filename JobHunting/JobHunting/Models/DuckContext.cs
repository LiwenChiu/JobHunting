using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Models;

public partial class DuckContext : DbContext
{
    public DuckContext()
    {
    }

    public DuckContext(DbContextOptions<DuckContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

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
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminID).HasName("PK__Admins__719FE4E8820EE8CD");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(16);
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateID).HasName("PK__Candidat__DF539BFC26C08296");

            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Degree).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.EmploymentStatus).HasMaxLength(20);
            entity.Property(e => e.MilitaryService).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.NationalId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("NationalID");
            entity.Property(e => e.Password).HasMaxLength(16);
            entity.Property(e => e.Phone).HasMaxLength(24);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyID).HasName("PK__Companie__2D971C4C3C587FA7");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Benefits).HasMaxLength(200);
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e => e.ContactEmail).HasMaxLength(320);
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactPhone).HasMaxLength(24);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Guinumber)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("GUINumber");
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(16);
            entity.Property(e => e.TitleClassId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("TitleClassID");

            entity.HasOne(d => d.TitleClass).WithMany(p => p.Companies)
                .HasForeignKey(d => d.TitleClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Companies__Title__29572725");
        });

        modelBuilder.Entity<CompanyOrder>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__CompanyO__C3905BAFA49D2453");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e => e.Guinumber)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("GUINumber");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Title).HasMaxLength(40);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__Compa__5812160E");

            entity.HasOne(d => d.Plan).WithMany(p => p.CompanyOrders)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__CompanyOr__PlanI__59063A47");
        });

        modelBuilder.Entity<CompanyResumeRecord>(entity =>
        {
            entity.HasKey(e => new { e.CompanyID, e.ResumeID }).HasName("PK__CompanyR__40EA667DACA8BDB6");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.HireYn).HasColumnName("HireYN");
            entity.Property(e => e.InterviewYn).HasColumnName("InterviewYN");
            entity.Property(e => e.LikeYn).HasColumnName("LikeYN");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyRe__Compa__403A8C7D");

            entity.HasOne(d => d.Resume).WithMany(p => p.CompanyResumeRecords)
                .HasForeignKey(d => d.ResumeId)
                .HasConstraintName("FK__CompanyRe__Resum__412EB0B6");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationID).HasName("PK__Notifica__20CF2E32AA267DB8");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.AppointmentTime).HasColumnType("datetime");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.SubjectLine).HasMaxLength(60);

            entity.HasOne(d => d.Candidate).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Candi__5EBF139D");

            entity.HasOne(d => d.Company).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__Compa__5DCAEF64");
        });

        modelBuilder.Entity<Opening>(entity =>
        {
            entity.HasKey(e => e.OpeningID).HasName("PK__Openings__808F87138AD1AE6F");

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
            entity.Property(e => e.Degree).HasMaxLength(20);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.SalaryMax).HasColumnType("money");
            entity.Property(e => e.SalaryMin).HasColumnType("money");
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title).HasMaxLength(60);
            entity.Property(e => e.TitleClassId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("TitleClassID");

            entity.HasOne(d => d.Company).WithMany(p => p.Openings)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Openings__Compan__2D27B809");

            entity.HasOne(d => d.TitleClass).WithMany(p => p.Openings)
                .HasForeignKey(d => d.TitleClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Openings__TitleC__2E1BDC42");

            entity.HasMany(d => d.Tags).WithMany(p => p.Openings)
                .UsingEntity<Dictionary<string, object>>(
                    "OpeningTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__OpeningTa__TagID__4BAC3F29"),
                    l => l.HasOne<Opening>().WithMany()
                        .HasForeignKey("OpeningId")
                        .HasConstraintName("FK__OpeningTa__Openi__4AB81AF0"),
                    j =>
                    {
                        j.HasKey("OpeningID", "TagID").HasName("PK__OpeningT__56D848B7AFAC992B");
                        j.ToTable("OpeningTags");
                        j.IndexerProperty<int>("OpeningId").HasColumnName("OpeningID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<OpinionLetter>(entity =>
        {
            entity.HasKey(e => e.LetterID).HasName("PK__OpinionL__AE46E811B1F3A15F");

            entity.Property(e => e.LetterId).HasColumnName("LetterID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Class).HasMaxLength(30);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.SubjectLine).HasMaxLength(60);

            entity.HasOne(d => d.Admin).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Admin__656C112C");

            entity.HasOne(d => d.Candidate).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Candi__6477ECF3");

            entity.HasOne(d => d.Company).WithMany(p => p.OpinionLetters)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__OpinionLe__Compa__6383C8BA");
        });

        modelBuilder.Entity<PricingPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__PricingP__755C22D7B9409A5E");

            entity.Property(e => e.PlanId).HasColumnName("PlanID");
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
            entity.HasKey(e => e.ResumeID).HasName("PK__Resumes__D7D7A3174E04D12B");

            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Intro).HasMaxLength(200);
            entity.Property(e => e.ReleaseYn)
                .HasDefaultValue(true)
                .HasColumnName("ReleaseYN");
            entity.Property(e => e.Time).HasMaxLength(60);
            entity.Property(e => e.Title).HasMaxLength(60);
            entity.Property(e => e.TitleClassId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("TitleClassID");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__Resumes__Candida__34C8D9D1");

            entity.HasOne(d => d.TitleClass).WithMany(p => p.Resumes)
                .HasForeignKey(d => d.TitleClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Resumes__TitleCl__35BCFE0A");

            entity.HasMany(d => d.Tags).WithMany(p => p.Resumes)
                .UsingEntity<Dictionary<string, object>>(
                    "ResumeTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__ResumeTag__TagID__4F7CD00D"),
                    l => l.HasOne<Resume>().WithMany()
                        .HasForeignKey("ResumeId")
                        .HasConstraintName("FK__ResumeTag__Resum__4E88ABD4"),
                    j =>
                    {
                        j.HasKey("ResumeID", "TagID").HasName("PK__ResumeTa__01806CB34329DD7C");
                        j.ToTable("ResumeTags");
                        j.IndexerProperty<int>("ResumeId").HasColumnName("ResumeID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<ResumeOpeningRecord>(entity =>
        {
            entity.HasKey(e => e.ResumeOpeningRecordID).HasName("PK__ResumeOp__CD5B6F3A435A8ED9");

            entity.Property(e => e.ResumeOpeningRecordId).HasColumnName("ResumeOpeningRecordID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName).HasMaxLength(40);
            entity.Property(e => e.HireYn).HasColumnName("HireYN");
            entity.Property(e => e.InterviewYn).HasColumnName("InterviewYN");
            entity.Property(e => e.LikeYn).HasColumnName("LikeYN");
            entity.Property(e => e.OpeningId).HasColumnName("OpeningID");
            entity.Property(e => e.OpeningTitle).HasMaxLength(60);
            entity.Property(e => e.ResumeId).HasColumnName("ResumeID");

            entity.HasOne(d => d.Opening).WithMany(p => p.ResumeOpeningRecords)
                .HasForeignKey(d => d.OpeningId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ResumeOpe__Openi__3A81B327");

            entity.HasOne(d => d.Resume).WithMany(p => p.ResumeOpeningRecords)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ResumeOpe__Resum__398D8EEE");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagID).HasName("PK__Tags__657CFA4C30210516");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.TagClassId)
                .HasDefaultValue(0)
                .HasColumnName("TagClassID");
            entity.Property(e => e.TagName).HasMaxLength(30);

            entity.HasOne(d => d.TagClass).WithMany(p => p.Tags)
                .HasForeignKey(d => d.TagClassId)
                .HasConstraintName("FK__Tags__TagClassID__47DBAE45");
        });

        modelBuilder.Entity<TagClass>(entity =>
        {
            entity.HasKey(e => e.TagClassID).HasName("PK__TagClass__94933B002831295A");

            entity.Property(e => e.TagClassId).HasColumnName("TagClassID");
            entity.Property(e => e.TagClassName).HasMaxLength(30);
        });

        modelBuilder.Entity<TitleCategory>(entity =>
        {
            entity.HasKey(e => e.TitleCategoryID).HasName("PK__TitleCat__079D34D85B7514A6");

            entity.Property(e => e.TitleCategoryId)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("TitleCategoryID");
            entity.Property(e => e.TitleCategoryName).HasMaxLength(40);
        });

        modelBuilder.Entity<TitleClass>(entity =>
        {
            entity.HasKey(e => e.TitleClassID).HasName("PK__TitleCla__7CC2BED94F2BBF32");

            entity.Property(e => e.TitleClassId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("TitleClassID");
            entity.Property(e => e.TitleCategoryId)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("TitleCategoryID");
            entity.Property(e => e.TitleClassName).HasMaxLength(40);

            entity.HasOne(d => d.TitleCategory).WithMany(p => p.TitleClasses)
                .HasForeignKey(d => d.TitleCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TitleClas__Title__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
