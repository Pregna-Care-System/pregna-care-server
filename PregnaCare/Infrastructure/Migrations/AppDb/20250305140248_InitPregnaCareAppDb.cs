using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PregnaCare.Migrations
{
    /// <inheritdoc />
    public partial class InitPregnaCareAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FeatureName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Feature__3214EC078D774F65", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "GrowthMetric",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    MinValue = table.Column<double>(type: "float", nullable: true),
                    MaxValue = table.Column<double>(type: "float", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__GrowthMe__3214EC079F959C7D", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "MembershipPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Membersh__3214EC0759A84688", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Pending"),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Notifica__3214EC07CC6FDA03", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "ReminderType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Reminder__3214EC07BCC21D48", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RoleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Role__3214EC070B95F870", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Tag__3214EC076482AF67", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FullName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, defaultValue: ""),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: ""),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__User__3214EC0783A4FABB", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "MembershipPlanFeature",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MembershipPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Membersh__3214EC075093C74B", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__Membershi__Featu__503BEA1C",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__Membershi__Membe__4F47C5E3",
                        column: x => x.MembershipPlanId,
                        principalTable: "MembershipPlan",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReminderTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    ReminderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Active"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Reminder__3214EC07D1B5B42B", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__Reminder__Remind__778AC167",
                        column: x => x.ReminderTypeId,
                        principalTable: "ReminderType",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Heading = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    ShortDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    FeaturedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    UrlHandle = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    IsVisible = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ViewCount = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Blog__3214EC072531253D", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__Blog__UserId__245D67DE",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "MotherInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    BloodType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValue: ""),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__MotherIn__3214EC07333353E5", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_MotherInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "UserMembershipPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__UserMemb__3214EC07B4262E50", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__UserMembe__Membe__57DD0BE4",
                        column: x => x.MembershipPlanId,
                        principalTable: "MembershipPlan",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__UserMembe__UserI__56E8E7AB",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__UserRole__3214EC071336C9DE", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__UserRole__RoleId__5EBF139D",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__UserRole__UserId__5DCAEF64",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "UserReminder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReminderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__UserRemi__3214EC07CD4B0FB8", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__UserRemin__Remin__02084FDA",
                        column: x => x.ReminderId,
                        principalTable: "Reminder",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__UserRemin__UserI__01142BA1",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "BlogTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__BlogTag__3214EC07DB6D1A4E", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__BlogTag__BlogId__690797E6",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__BlogTag__TagId__69FBBC1F",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Comment__3214EC07AA0617CF", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__Comment__BlogId__32AB8735",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__Comment__ParentC__3493CFA7",
                        column: x => x.ParentCommentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__Comment__UserId__339FAB6E",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "PregnancyRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MotherInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BabyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    PregnancyStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpectedDueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BabyGender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__Pregnanc__3214EC0744B8C9AF", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__Pregnancy__MotherInfoI__656C112C",
                        column: x => x.MotherInfoId,
                        principalTable: "MotherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "FetalGrowthRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PregnancyRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Week = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__FetalGro__3214EC0770636744", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__FetalGrow__Pregn__19DFD96B",
                        column: x => x.PregnancyRecordId,
                        principalTable: "PregnancyRecord",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "GrowthAlert",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FetalGrowthRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: true),
                    AlertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AlertFor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Fetal"),
                    Issue = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK__GrowthAl__3214EC07E416DF1F", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK__GrowthAle__Fetal__7B264821",
                        column: x => x.FetalGrowthRecordId,
                        principalTable: "FetalGrowthRecord",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK__GrowthAle__UserI__7C1A6C5A",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Blog_UserId",
                table: "Blog",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_BlogTag_BlogId",
                table: "BlogTag",
                column: "BlogId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagId",
                table: "BlogTag",
                column: "TagId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogId",
                table: "Comment",
                column: "BlogId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommentId",
                table: "Comment",
                column: "ParentCommentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "UQ__Feature__55ABBB7126C564CB",
                table: "Feature",
                column: "FeatureName",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_FetalGrowthRecord_PregnancyRecordId",
                table: "FetalGrowthRecord",
                column: "PregnancyRecordId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_GrowthAlert_FetalGrowthRecordId",
                table: "GrowthAlert",
                column: "FetalGrowthRecordId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_GrowthAlert_UserId",
                table: "GrowthAlert",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MembershipPlanFeature_FeatureId",
                table: "MembershipPlanFeature",
                column: "FeatureId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MembershipPlanFeature_MembershipPlanId",
                table: "MembershipPlanFeature",
                column: "MembershipPlanId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_MotherInfo_UserId",
                table: "MotherInfo",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PregnancyRecord_MotherInfoId",
                table: "PregnancyRecord",
                column: "MotherInfoId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reminder_ReminderTypeId",
                table: "Reminder",
                column: "ReminderTypeId");

            _ = migrationBuilder.CreateIndex(
                name: "UQ__Tag__737584F6E702CB57",
                table: "Tag",
                column: "Name",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "UQ__User__A9D105349BB5D819",
                table: "User",
                column: "Email",
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserMembershipPlan_MembershipPlanId",
                table: "UserMembershipPlan",
                column: "MembershipPlanId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserMembershipPlan_UserId",
                table: "UserMembershipPlan",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserReminder_ReminderId",
                table: "UserReminder",
                column: "ReminderId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserReminder_UserId",
                table: "UserReminder",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "BlogTag");

            _ = migrationBuilder.DropTable(
                name: "Comment");

            _ = migrationBuilder.DropTable(
                name: "GrowthAlert");

            _ = migrationBuilder.DropTable(
                name: "GrowthMetric");

            _ = migrationBuilder.DropTable(
                name: "MembershipPlanFeature");

            _ = migrationBuilder.DropTable(
                name: "Notification");

            _ = migrationBuilder.DropTable(
                name: "UserMembershipPlan");

            _ = migrationBuilder.DropTable(
                name: "UserReminder");

            _ = migrationBuilder.DropTable(
                name: "UserRole");

            _ = migrationBuilder.DropTable(
                name: "Tag");

            _ = migrationBuilder.DropTable(
                name: "Blog");

            _ = migrationBuilder.DropTable(
                name: "FetalGrowthRecord");

            _ = migrationBuilder.DropTable(
                name: "Feature");

            _ = migrationBuilder.DropTable(
                name: "MembershipPlan");

            _ = migrationBuilder.DropTable(
                name: "Reminder");

            _ = migrationBuilder.DropTable(
                name: "Role");

            _ = migrationBuilder.DropTable(
                name: "PregnancyRecord");

            _ = migrationBuilder.DropTable(
                name: "ReminderType");

            _ = migrationBuilder.DropTable(
                name: "MotherInfo");

            _ = migrationBuilder.DropTable(
                name: "User");
        }
    }
}
