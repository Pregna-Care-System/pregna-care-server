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

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FetalGrowthRecord> FetalGrowthRecords { get; set; }

    public virtual DbSet<GrowthAlert> GrowthAlerts { get; set; }

    public virtual DbSet<GrowthMetric> GrowthMetrics { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<MembershipPlanFeature> MembershipPlanFeatures { get; set; }

    public virtual DbSet<MotherInfo> MotherInfos { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PregnancyRecord> PregnancyRecords { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<ReminderType> ReminderTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMembershipPlan> UserMembershipPlans { get; set; }

    public virtual DbSet<UserReminder> UserReminders { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC072531253D");

            entity.ToTable("Blog");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FeaturedImageUrl).HasDefaultValue("");
            entity.Property(e => e.Heading)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.Property(e => e.PageTitle)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrlHandle).HasDefaultValue("");
            entity.Property(e => e.ViewCount).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blog__UserId__245D67DE");
        });

        modelBuilder.Entity<BlogTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogTag__3214EC07DB6D1A4E");

            entity.ToTable("BlogTag");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__BlogId__690797E6");

            entity.HasOne(d => d.Tag).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__TagId__69FBBC1F");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07AA0617CF");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CommentText)
                .IsRequired()
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__BlogId__32AB8735");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__3493CFA7");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserId__339FAB6E");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC078D774F65");

            entity.ToTable("Feature");

            entity.HasIndex(e => e.FeatureName, "UQ__Feature__55ABBB7126C564CB").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.FeatureName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<FetalGrowthRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FetalGro__3214EC0770636744");

            entity.ToTable("FetalGrowthRecord");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Note).HasDefaultValue("");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PregnancyRecord).WithMany(p => p.FetalGrowthRecords)
                .HasForeignKey(d => d.PregnancyRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FetalGrow__Pregn__19DFD96B");
        });

        modelBuilder.Entity<GrowthAlert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GrowthAl__3214EC07E416DF1F");

            entity.ToTable("GrowthAlert");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AlertDate).HasColumnType("datetime");
            entity.Property(e => e.AlertFor)
                .HasMaxLength(20)
                .HasDefaultValue("Fetal");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsResolved).HasDefaultValue(false);
            entity.Property(e => e.Issue).HasDefaultValue("");
            entity.Property(e => e.Recommendation).HasDefaultValue("");
            entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FetalGrowthRecord).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.FetalGrowthRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__Fetal__7B264821");

            entity.HasOne(d => d.User).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__UserI__7C1A6C5A");
        });

        modelBuilder.Entity<GrowthMetric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GrowthMe__3214EC079F959C7D");

            entity.ToTable("GrowthMetric");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC0759A84688");

            entity.ToTable("MembershipPlan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.PlanName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MembershipPlanFeature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC075093C74B");

            entity.ToTable("MembershipPlanFeature");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Feature).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Featu__503BEA1C");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Membe__4F47C5E3");
        });

        modelBuilder.Entity<MotherInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MotherIn__3214EC07333353E5");

            entity.ToTable("MotherInfo");

            entity.HasIndex(e => e.PregnancyRecordId, "UQ__MotherIn__06D3067E0B86D5C8").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BloodType)
                .HasMaxLength(2)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HealthStatus).HasDefaultValue("");
            entity.Property(e => e.MotherName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.Notes).HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PregnancyRecord).WithOne(p => p.MotherInfo)
                .HasForeignKey<MotherInfo>(d => d.PregnancyRecordId)
                .HasConstraintName("FK__MotherInf__Pregn__719CDDE7");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07CC6FDA03");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Message).HasDefaultValue("");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PregnancyRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pregnanc__3214EC0744B8C9AF");

            entity.ToTable("PregnancyRecord");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BabyGender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            entity.Property(e => e.BabyName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.PregnancyRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Pregnancy__UserI__656C112C");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07D1B5B42B");

            entity.ToTable("Reminder");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ReminderDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ReminderType).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.ReminderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reminder__Remind__778AC167");
        });

        modelBuilder.Entity<ReminderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07BCC21D48");

            entity.ToTable("ReminderType");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070B95F870");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC076482AF67");

            entity.ToTable("Tag");

            entity.HasIndex(e => e.Name, "UQ__Tag__737584F6E702CB57").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0783A4FABB");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105349BB5D819").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(60);
            entity.Property(e => e.FullName)
                .HasMaxLength(60)
                .HasDefaultValue("");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            entity.Property(e => e.ImageUrl).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserMembershipPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMemb__3214EC07B4262E50");

            entity.ToTable("UserMembershipPlan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ActivatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__Membe__57DD0BE4");

            entity.HasOne(d => d.User).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__UserI__56E8E7AB");
        });

        modelBuilder.Entity<UserReminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRemi__3214EC07CD4B0FB8");

            entity.ToTable("UserReminder");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Reminder).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.ReminderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__Remin__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__UserI__01142BA1");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC071336C9DE");

            entity.ToTable("UserRole");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRole__RoleId__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserId__5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
