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

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FetalGrowthRecord> FetalGrowthRecords { get; set; }

    public virtual DbSet<FetalStandard> FetalStandards { get; set; }

    public virtual DbSet<GrowthAlert> GrowthAlerts { get; set; }

    public virtual DbSet<GrowthMetric> GrowthMetrics { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PregnancyRecord> PregnancyRecords { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<ReminderType> ReminderTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMembershipPlan> UserMembershipPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AlertAct__3214EC07C2A4D44D");

            entity.ToTable("AlertAction");

            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.PerformedBy).HasMaxLength(50);

            entity.HasOne(d => d.GrowthAlert).WithMany(p => p.AlertActions)
                .HasForeignKey(d => d.GrowthAlertId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AlertAction_GrowthAlert");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC073FD8917E");

            entity.ToTable("Blog");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Heading).HasMaxLength(100);
            entity.Property(e => e.PageTitle).HasMaxLength(100);
            entity.Property(e => e.ShortDescription).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Blog_User");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07B00D0F61");

            entity.ToTable("Comment");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_Comment_Blog");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK_Comment_Parent");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC07496EF255");

            entity.ToTable("Feature");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.FeatureName).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<FetalGrowthRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FetalGro__3214EC07EEB5B9B9");

            entity.ToTable("FetalGrowthRecord");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.GrowthMetric).WithMany(p => p.FetalGrowthRecords)
                .HasForeignKey(d => d.GrowthMetricId)
                .HasConstraintName("FK_FetalGrowthRecord_GrowthMetric");

            entity.HasOne(d => d.PregnancyRecord).WithMany(p => p.FetalGrowthRecords)
                .HasForeignKey(d => d.PregnancyRecordId)
                .HasConstraintName("FK_FetalGrowthRecord_PregnancyRecord");
        });

        modelBuilder.Entity<FetalStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FetalSta__3214EC071C599EA3");

            entity.ToTable("FetalStandard");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.GrowthMetric).WithMany(p => p.FetalStandards)
                .HasForeignKey(d => d.GrowthMetricId)
                .HasConstraintName("FK_FetalStandards_GrowthMetric");
        });

        modelBuilder.Entity<GrowthAlert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GrowthAl__3214EC07BA4054B3");

            entity.ToTable("GrowthAlert");

            entity.Property(e => e.AlertDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.GrowthMetric).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.GrowthMetricId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GrowthAlert_GrowthMetric");

            entity.HasOne(d => d.User).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GrowthAlert_User");
        });

        modelBuilder.Entity<GrowthMetric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GrowthMe__3214EC070473A897");

            entity.ToTable("GrowthMetric");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC07B1FE5651");

            entity.ToTable("MembershipPlan");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.PlanName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasMany(d => d.Features).WithMany(p => p.MembershipPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "MembershipPlanFeature",
                    r => r.HasOne<Feature>().WithMany()
                        .HasForeignKey("FeatureId")
                        .HasConstraintName("FK_MembershipPlanFeature_Feature"),
                    l => l.HasOne<MembershipPlan>().WithMany()
                        .HasForeignKey("MembershipPlanId")
                        .HasConstraintName("FK_MembershipPlanFeature_MembershipPlan"),
                    j =>
                    {
                        j.HasKey("MembershipPlanId", "FeatureId").HasName("PK__Membersh__06667B0ADD040FDD");
                        j.ToTable("MembershipPlanFeature");
                    });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC070986A782");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Reminder).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReminderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Notification_Reminder");
        });

        modelBuilder.Entity<PregnancyRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pregnanc__3214EC0709586842");

            entity.ToTable("PregnancyRecord");

            entity.Property(e => e.BabyGender)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BabyName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MotherBloodType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.PregnancyRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PregnancyRecord_User");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC078A29B6E1");

            entity.ToTable("Reminder");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ReminderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ReminderType).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.ReminderTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Reminder_ReminderType");

            entity.HasOne(d => d.User).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Reminder_User");
        });

        modelBuilder.Entity<ReminderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07F33E4C8E");

            entity.ToTable("ReminderType");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TypeName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC078F8A9275");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(40);
            entity.Property(e => e.FullName).HasMaxLength(60);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserMembershipPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMemb__3214EC07F1F4FCD7");

            entity.ToTable("UserMembershipPlan");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserMembershipPlan_MembershipPlan");

            entity.HasOne(d => d.User).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserMembershipPlan_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
