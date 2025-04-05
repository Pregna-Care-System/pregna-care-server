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

    public virtual DbSet<Reaction> Reactions { get; set; }

    public DbSet<FAQ> FAQs { get; set; }

    public DbSet<FAQCategory> FAQCategories { get; set; }

    public DbSet<FeedBack> FeedBacks { get; set; }

    public DbSet<ContactSubscriber> ContactSubscribers { get; set; }

    public DbSet<WeeklyRecommendation> WeeklyRecommendations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Blog>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC072531253D");

            _ = entity.ToTable("Blog");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.Content).HasDefaultValue("");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.FeaturedImageUrl).HasDefaultValue("");
            _ = entity.Property(e => e.Heading)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.IsVisible).HasDefaultValue(true);
            _ = entity.Property(e => e.PageTitle)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.ShortDescription)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.UrlHandle).HasDefaultValue("");
            _ = entity.Property(e => e.ViewCount).HasDefaultValue(0);

            _ = entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blog__UserId__245D67DE");
        });

        _ = modelBuilder.Entity<BlogTag>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__BlogTag__3214EC07DB6D1A4E");

            _ = entity.ToTable("BlogTag");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.Blog).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__BlogId__690797E6");

            _ = entity.HasOne(d => d.Tag).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__TagId__69FBBC1F");
        });

        _ = modelBuilder.Entity<Comment>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07AA0617CF");

            _ = entity.ToTable("Comment");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CommentText)
                .IsRequired()
                .HasDefaultValue("");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__BlogId__32AB8735");

            _ = entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__3493CFA7");

            _ = entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserId__339FAB6E");
        });

        _ = modelBuilder.Entity<Feature>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC078D774F65");

            _ = entity.ToTable("Feature");

            _ = entity.HasIndex(e => e.FeatureName, "UQ__Feature__55ABBB7126C564CB").IsUnique();

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.FeatureName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<FetalGrowthRecord>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__FetalGro__3214EC0770636744");

            _ = entity.ToTable("FetalGrowthRecord");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");
            _ = entity.Property(e => e.Note).HasDefaultValue("");
            _ = entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.PregnancyRecord).WithMany(p => p.FetalGrowthRecords)
                .HasForeignKey(d => d.PregnancyRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FetalGrow__Pregn__19DFD96B");
        });

        _ = modelBuilder.Entity<GrowthAlert>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__GrowthAl__3214EC07E416DF1F");

            _ = entity.ToTable("GrowthAlert");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.AlertDate).HasColumnType("datetime");
            _ = entity.Property(e => e.AlertFor)
                .HasMaxLength(20)
                .HasDefaultValue("Fetal");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.IsResolved).HasDefaultValue(false);
            _ = entity.Property(e => e.Issue).HasDefaultValue("");
            _ = entity.Property(e => e.Recommendation).HasDefaultValue("");
            _ = entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.FetalGrowthRecord).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.FetalGrowthRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__Fetal__7B264821");

            _ = entity.HasOne(d => d.User).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__UserI__7C1A6C5A");
        });

        _ = modelBuilder.Entity<GrowthMetric>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__GrowthMe__3214EC079F959C7D");

            _ = entity.ToTable("GrowthMetric");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");
            _ = entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<MembershipPlan>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC0759A84688");

            _ = entity.ToTable("MembershipPlan");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.PlanName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<MembershipPlanFeature>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC075093C74B");

            _ = entity.ToTable("MembershipPlanFeature");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.Feature).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Featu__503BEA1C");

            _ = entity.HasOne(d => d.MembershipPlan).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Membe__4F47C5E3");
        });

        _ = modelBuilder.Entity<MotherInfo>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__MotherIn__3214EC07333353E5");

            _ = entity.ToTable("MotherInfo");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.BloodType)
                .HasMaxLength(2)
                .HasDefaultValue("");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.HealthStatus).HasDefaultValue("");
            _ = entity.Property(e => e.Notes).HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasMany(m => m.PregnancyRecords)
                .WithOne(p => p.MotherInfo)
                .HasForeignKey(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        _ = modelBuilder.Entity<Notification>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07CC6FDA03");

            _ = entity.ToTable("Notification");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.Message).HasDefaultValue("");
            _ = entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<PregnancyRecord>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Pregnanc__3214EC0744B8C9AF");

            _ = entity.ToTable("PregnancyRecord");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.BabyGender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            _ = entity.Property(e => e.BabyName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.MotherInfo)
                      .WithMany(p => p.PregnancyRecords)
                      .HasForeignKey(d => d.MotherInfoId)
                      .HasConstraintName("FK__Pregnancy__MotherInfoI__656C112C");
        });

        _ = modelBuilder.Entity<Reminder>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07D1B5B42B");

            _ = entity.ToTable("Reminder");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.ReminderDate).HasColumnType("datetime");
            _ = entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.ReminderType).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.ReminderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reminder__Remind__778AC167");
        });

        _ = modelBuilder.Entity<ReminderType>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07BCC21D48");

            _ = entity.ToTable("ReminderType");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<Role>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070B95F870");

            _ = entity.ToTable("Role");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<Tag>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC076482AF67");

            _ = entity.ToTable("Tag");

            _ = entity.HasIndex(e => e.Name, "UQ__Tag__737584F6E702CB57").IsUnique();

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Description).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<User>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__User__3214EC0783A4FABB");

            _ = entity.ToTable("User");

            _ = entity.HasIndex(e => e.Email, "UQ__User__A9D105349BB5D819").IsUnique();

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.Email).HasMaxLength(60);
            _ = entity.Property(e => e.FullName)
                .HasMaxLength(60)
                .HasDefaultValue("");
            _ = entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            _ = entity.Property(e => e.ImageUrl).HasDefaultValue("");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.Password).IsUnicode(false);
            _ = entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("");
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(u => u.MotherInfo)
                      .WithOne(m => m.User)
                      .HasForeignKey<MotherInfo>(m => m.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
        });

        _ = modelBuilder.Entity<UserMembershipPlan>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__UserMemb__3214EC07B4262E50");

            _ = entity.ToTable("UserMembershipPlan");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.ActivatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            _ = entity.Property(e => e.IsActive).HasDefaultValue(false);
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.MembershipPlan).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__Membe__57DD0BE4");

            _ = entity.HasOne(d => d.User).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__UserI__56E8E7AB");
        });

        _ = modelBuilder.Entity<UserReminder>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__UserRemi__3214EC07CD4B0FB8");

            _ = entity.ToTable("UserReminder");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.Reminder).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.ReminderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__Remin__02084FDA");

            _ = entity.HasOne(d => d.User).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__UserI__01142BA1");
        });

        _ = modelBuilder.Entity<UserRole>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC071336C9DE");

            _ = entity.ToTable("UserRole");

            _ = entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            _ = entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            _ = entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            _ = entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            _ = entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRole__RoleId__5EBF139D");

            _ = entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserId__5DCAEF64");
        });

        _ = modelBuilder.Entity<FAQ>()
                    .HasOne(f => f.Category)
                    .WithMany(c => c.FAQs)
                    .HasForeignKey(f => f.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

        _ = modelBuilder.Entity<FAQ>()
                    .Property(f => f.Question)
                    .IsRequired()
                    .HasMaxLength(500);

        _ = modelBuilder.Entity<FAQ>()
                    .Property(f => f.Answer)
                    .IsRequired();

        _ = modelBuilder.Entity<FAQCategory>()
                    .Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

        _ = modelBuilder.Entity<FeedBack>(entity =>
        {
            _ = entity.HasKey(e => e.Id);

            _ = entity.Property(e => e.UserId)
                  .IsRequired();

            _ = entity.Property(e => e.Rating)
                  .IsRequired();

            _ = entity.Property(e => e.Content);

            _ = entity.Property(e => e.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETDATE()");

            _ = entity.Property(e => e.UpdatedAt);

            _ = entity.Property(e => e.IsDeleted)
                  .IsRequired()
                  .HasDefaultValue(false);

            _ = entity.HasOne(e => e.User)
                  .WithMany(u => u.Feedbacks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<WeeklyRecommendation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(e => e.PregnancyRecord)
                .WithMany()
                .HasForeignKey(e => e.PregnancyRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}