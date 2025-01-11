using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;

namespace PregnaCare.Infrastructure.Data;

public partial class PregnaCareAppDbContext : DbContext
{
    public PregnaCareAppDbContext()
    {
    }

    public PregnaCareAppDbContext(DbContextOptions<PregnaCareAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlertAction> AlertActions { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<GrowthAlert> GrowthAlerts { get; set; }

    public virtual DbSet<GrowthMetric> GrowthMetrics { get; set; }

    public virtual DbSet<GrowthStandard> GrowthStandards { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<MembershipPlanHistory> MembershipPlanHistories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PregnancyRecord> PregnancyRecords { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<ReminderType> ReminderTypes { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertAction>(entity =>
        {
            entity.ToTable("AlertAction");

            entity.Property(e => e.AlertActionId).HasColumnName("AlertActionID");
            entity.Property(e => e.ActionType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.AlertId).HasColumnName("AlertID");
            entity.Property(e => e.PerformedBy)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Alert).WithMany(p => p.AlertActions)
                .HasForeignKey(d => d.AlertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlertAction_GrowthAlert");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.PageTitle).HasMaxLength(50);
            entity.Property(e => e.PublishedDate).HasColumnType("datetime");
            entity.Property(e => e.ShortDescription).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Author).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blog_UserAccount");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CommentDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_Comment_Blog");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("FK_Comment_UserAccount");
        });

        modelBuilder.Entity<GrowthAlert>(entity =>
        {
            entity.HasKey(e => e.AlertId);

            entity.ToTable("GrowthAlert");

            entity.Property(e => e.AlertId).HasColumnName("AlertID");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.GrowthMetricId).HasColumnName("GrowthMetricID");
            entity.Property(e => e.IsResolved).HasDefaultValue(false);
            entity.Property(e => e.Issue).IsRequired();
            entity.Property(e => e.Severity)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.GrowthMetric).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.GrowthMetricId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GrowthAlert_GrowthMetric");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GrowthAlert_UserAccount");
        });

        modelBuilder.Entity<GrowthMetric>(entity =>
        {
            entity.ToTable("GrowthMetric");

            entity.Property(e => e.GrowthMetricId).HasColumnName("GrowthMetricID");
            entity.Property(e => e.AmnioticFluidVolume).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.FetalHeight).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.FetalMovement)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FetalStructuralAbnormalities)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FetalWeight).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.GestationalDiabetes)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MaternalBloodGlucose).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MaternalBloodPressure)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.MaternalWeightGain).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PreEclampsia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PregnancyRecordId).HasColumnName("PregnancyRecordID");
            entity.Property(e => e.PregnancyStatus).HasMaxLength(50);

            entity.HasOne(d => d.PregnancyRecord).WithMany(p => p.GrowthMetrics)
                .HasForeignKey(d => d.PregnancyRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GrowthMetric_PregnancyRecord");
        });

        modelBuilder.Entity<GrowthStandard>(entity =>
        {
            entity.ToTable("GrowthStandard");

            entity.Property(e => e.GrowthStandardId).HasColumnName("GrowthStandardID");
            entity.Property(e => e.Category)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Indicator)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MaxStandard).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MinStandard).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.WarningMessage)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.ToTable("MembershipPlan");

            entity.Property(e => e.MembershipPlanId).HasColumnName("MembershipPlanID");
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<MembershipPlanHistory>(entity =>
        {
            entity.HasKey(e => e.PlanHistoryId);

            entity.ToTable("MembershipPlanHistory");

            entity.Property(e => e.PlanHistoryId).HasColumnName("PlanHistoryID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.Payment).WithMany(p => p.MembershipPlanHistories)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_MembershipPlanHistory_Payment");

            entity.HasOne(d => d.Plan).WithMany(p => p.MembershipPlanHistories)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_MembershipPlanHistory_MembershipPlan");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.MembershipPlanHistories)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("FK_MembershipPlanHistory_UserAccount");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ReadAt).HasColumnType("datetime");
            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.SentAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Reminder).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReminderId)
                .HasConstraintName("FK_Notification_Reminder");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PaymentDateTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_UserAccount");
        });

        modelBuilder.Entity<PregnancyRecord>(entity =>
        {
            entity.ToTable("PregnancyRecord");

            entity.Property(e => e.PregnancyRecordId).HasColumnName("PregnancyRecordID");
            entity.Property(e => e.BabyName).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(15);
            entity.Property(e => e.FatherName).HasMaxLength(50);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MotherName).HasMaxLength(50);
            entity.Property(e => e.PregnancyType).HasMaxLength(50);
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.PregnancyRecords)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("FK_PregnancyRecord_UserAccount");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.ToTable("Reminder");

            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ReminderDate).HasColumnType("datetime");
            entity.Property(e => e.ReminderTypeId).HasColumnName("ReminderTypeID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");

            entity.HasOne(d => d.ReminderType).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.ReminderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reminder_ReminderType");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("FK_Reminder_UserAccount");
        });

        modelBuilder.Entity<ReminderType>(entity =>
        {
            entity.ToTable("ReminderType");

            entity.Property(e => e.ReminderTypeId).HasColumnName("ReminderTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.ToTable("UserAccount");

            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
