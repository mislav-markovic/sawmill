using Microsoft.EntityFrameworkCore;

namespace SawMill.Data.Models
{
  public class SawMillDbContext : DbContext
  {
    public SawMillDbContext()
    {
      ChangeTracker.QueryTrackingBehavior =
        QueryTrackingBehavior.NoTracking;
    }

    public SawMillDbContext(DbContextOptions<SawMillDbContext> options)
      : base(options)
    {
    }

    public virtual DbSet<Alert> Alert { get; set; }

    public virtual DbSet<AlertValue> AlertValue { get; set; }
    public virtual DbSet<AlertGroup> AlertGroup { get; set; }
    public virtual DbSet<AlertGroupValue> AlertGroupValue { get; set; }

    public virtual DbSet<Component> Component { get; set; }
    public virtual DbSet<CustomAttributeValue> CustomAttributeValue { get; set; }
    public virtual DbSet<DateTimeRule> DateTimeRule { get; set; }
    public virtual DbSet<GeneralRule> GeneralRule { get; set; }
    public virtual DbSet<MessageRule> MessageRule { get; set; }
    public virtual DbSet<NormalizedLog> NormalizedLog { get; set; }
    public virtual DbSet<ParsingRules> ParsingRules { get; set; }
    public virtual DbSet<RawLog> RawLog { get; set; }
    public virtual DbSet<SeverityLevel> SeverityLevel { get; set; }
    public virtual DbSet<SeverityLevelRule> SeverityLevelRule { get; set; }
    public virtual DbSet<System> System { get; set; }
    public virtual DbSet<CustomAttributeRuleParsingRules> CustomAttributeRuleParsingRules { get; set; }
    public virtual DbSet<SystemReport> SystemReport { get; set; }
    public virtual DbSet<SystemReportAlertGroup> SystemReportAlertGroup { get; set; }
    public virtual DbSet<ComponentReport> ComponentReport { get; set; }
    public virtual DbSet<ComponentReportAlert> ComponentReportAlert { get; set; }
    public virtual DbSet<Settings> Settings { get; set; }
    public virtual DbSet<AlertGroupAlert> AlertGroupAlert { get; set; }
    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        optionsBuilder.UseSqlServer("Server=DESKTOP-U303OM7\\SQLEXPRESS;Database=sawmill.db;Trusted_Connection=True;");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

      modelBuilder.Entity<User>(entity =>
      {
        entity.Property(e => e.UserId).ValueGeneratedOnAdd();

        entity.HasData(
          new User{UserId = 1, Category = "guest", Name = "User A"},
          new User { UserId = 2, Category = "guest", Name = "User B" },
          new User { UserId = 3, Category = "admin", Name = "User C" });
      });

      modelBuilder.Entity<Settings>(entity =>
      {
        entity.Property(e => e.SettingsId).ValueGeneratedOnAdd();

        entity.HasData(new Settings {SettingsId = 1, Frequency = 60, Name = "Analyzer Frequency"});
      });

      modelBuilder.Entity<SystemReport>(entity =>
      {
        entity.Property(e => e.SystemReportId).ValueGeneratedOnAdd();
        entity.Property(e => e.Timestamp).HasColumnType("datetime");

        entity.HasOne(e => e.System)
          .WithMany(e => e.SystemReports)
          .HasForeignKey(e => e.SystemId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_System_SystemReport")
          .IsRequired();
      });

      modelBuilder.Entity<SystemReportAlertGroup>(entity =>
      {
        entity.Property(e => e.SystemReportAlertGroupId).ValueGeneratedOnAdd();

        entity.HasOne(e => e.SystemReport)
          .WithMany(e => e.SystemReportAlertGroup)
          .HasForeignKey(e => e.SystemReportId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_SystemReport_SystemReportAlertGroup")
          .IsRequired();

        entity.HasOne(e => e.AlertGroup)
          .WithMany(e => e.SystemReportAlertGroups)
          .HasForeignKey(e => e.AlertGroupId)
          .HasConstraintName("FK_AlertGroup_SystemReportAlertGroup")
          .IsRequired();
      });

      modelBuilder.Entity<ComponentReport>(entity =>
      {
        entity.Property(e => e.ComponentReportId).ValueGeneratedOnAdd();
        entity.Property(e => e.Timestamp).HasColumnType("datetime");

        entity.HasOne(e => e.SystemReport)
          .WithMany(e => e.ComponentReports)
          .HasForeignKey(e => e.SystemReportId)
          .OnDelete(DeleteBehavior.Cascade)
          .HasConstraintName("FK_SystemReport_ComponentReport");

        entity.HasOne(e => e.Component)
          .WithMany(e => e.ComponentReports)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasForeignKey(e => e.ComponentId)
          .HasConstraintName("FK_Component_ComponentReports")
          .IsRequired();
      });

      modelBuilder.Entity<AlertGroupAlert>(entity =>
      {
        entity.HasKey(e => new {e.AlertId, e.AlertGroupId});

        entity.HasOne(e => e.Alert).WithMany(a => a.AlertGroupAlerts)
          .HasForeignKey(e => e.AlertId).OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Alert_AlertGroupAlert");

        entity.HasOne(e => e.AlertGroup).WithMany(a => a.AlertGroupAlert)
          .HasForeignKey(e => e.AlertGroupId).OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_AlertGroup_AlertGroupAlert");
      });

      modelBuilder.Entity<ComponentReportAlert>(entity =>
      {
        entity.Property(e => e.ComponentReportAlertId).ValueGeneratedOnAdd();

        entity.HasOne(e => e.ComponentReport)
          .WithMany(e => e.ComponentReportAlert)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasForeignKey(e => e.ComponentReportId)
          .HasConstraintName("FK_ComponentReport_ComponentReportAlert")
          .IsRequired();

        entity.HasOne(e => e.Alert)
          .WithMany(e => e.ComponentReportAlerts)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasForeignKey(e => e.AlertId)
          .HasConstraintName("FK_Alert_ComponentReportAlert")
          .IsRequired();
      });

      modelBuilder.Entity<Alert>(entity =>
      {
        entity.Property(c => c.AlertId).ValueGeneratedOnAdd();

        entity.Property(a => a.Value).IsRequired();

        entity.HasOne(alert => alert.GeneralRule).WithMany(d => d.Alerts)
          .HasForeignKey(d => d.GeneralRuleId).IsRequired().OnDelete(DeleteBehavior.Cascade)
          .HasConstraintName("FK_Alert_GeneralRule");


        entity.HasOne(alert => alert.Component).WithMany(d => d.Alerts).HasForeignKey(d => d.ComponentId).IsRequired()
          .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Alert_Component");
      });

      modelBuilder.Entity<AlertValue>(entity =>
      {
        entity.Property(av => av.AlertValueId).ValueGeneratedOnAdd();

        entity.Property(av => av.TimespanStart).HasColumnType("datetime");
        entity.Property(av => av.TimespanEnd).HasColumnType("datetime");

        entity.Property(av => av.ConstantValue).IsRequired(false);

        entity.HasOne(av => av.AlertGroupValue)
          .WithMany(agv => agv.AlertValues)
          .HasForeignKey(av => av.AlertGroupValueId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_AlertValue_AlertGroupValue")
          .IsRequired(false);

        entity.HasOne(av => av.Alert)
          .WithMany(a => a.AlertValues)
          .HasForeignKey(av => av.AlertId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_AlertValue_Alert");
      });

      modelBuilder.Entity<AlertGroup>(entity =>
      {
        entity.Property(av => av.AlertGroupId).ValueGeneratedOnAdd();

        entity.HasOne(ag => ag.System)
          .WithMany(s => s.AlertGroup)
          .HasForeignKey(ag => ag.SystemId)
          .HasConstraintName("FK_AlertGroup_System");
      });

      modelBuilder.Entity<AlertGroupValue>(entity =>
      {
        entity.Property(av => av.AlertGroupValueId).ValueGeneratedOnAdd();

        entity.Property(av => av.TimespanStart).HasColumnType("datetime");
        entity.Property(av => av.TimespanEnd).HasColumnType("datetime");

        entity.HasOne(agv => agv.AlertGroup)
          .WithMany(ag => ag.AlertGroupValues)
          .HasForeignKey(agv => agv.AlertGroupId)
          .HasConstraintName("FK_AlertGroup_AlertGroupValue");
      });

      modelBuilder.Entity<Component>(entity =>
      {
        entity.Property(c => c.ComponentId).ValueGeneratedOnAdd();

        entity.ForSqlServerHasIndex(e => e.ComponentName).IsUnique();

        entity.Property(e => e.ComponentName)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.HasOne(d => d.System)
          .WithMany(p => p.Component)
          .HasForeignKey(d => d.SystemId)
          .IsRequired(false)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Component_System");

        entity.HasOne(d => d.ParsingRules)
          .WithMany(d => d.Components)
          .HasForeignKey(d => d.ParsingRulesId)
          .IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Component_ParsingRules");
      });
      
      modelBuilder.Entity<CustomAttributeValue>(entity =>
      {
        entity.Property(c => c.CustomAttributeValueId).ValueGeneratedOnAdd();

        entity.Property(e => e.Value)
          .IsRequired()
          .HasMaxLength(2048)
          .IsUnicode(false);

        entity.HasOne(d => d.GeneralRule)
          .WithMany(p => p.CustomAttributeValues)
          .HasForeignKey(d => d.GeneralRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_CustomAttributeValue_GeneralRule");

        entity.HasOne(d => d.NormalizedLog)
          .WithMany(p => p.CustomAttributeValues)
          .HasForeignKey(d => d.NormalizedLogId)
          .OnDelete(DeleteBehavior.Cascade)
          .HasConstraintName("FK_CustomAttributeValue_NormalizedLog");
      });

      modelBuilder.Entity<DateTimeRule>(entity =>
      {
        entity.Property(c => c.DateTimeRuleId).ValueGeneratedOnAdd();

        entity.Property(e => e.DateTimeFormat)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.HasOne(d => d.GeneralRule)
          .WithMany(p => p.DateTimeRule)
          .HasForeignKey(d => d.GeneralRuleId)
          .OnDelete(DeleteBehavior.Cascade)
          .HasConstraintName("FK_DateTimeRule_GeneralRule");
      });

      modelBuilder.Entity<GeneralRule>(entity =>
      {
        entity.Property(c => c.GeneralRuleId).ValueGeneratedOnAdd();

        entity.Property(e => e.GeneralRuleDescription)
          .IsRequired()
          .HasMaxLength(4096)
          .IsUnicode(false);

        entity.Property(e => e.GeneralRuleEndAnchor)
          .HasMaxLength(1024)
          .IsUnicode(false);

        entity.Property(e => e.GeneralRuleMatcher)
          .IsRequired()
          .HasMaxLength(2048)
          .IsUnicode(false);

        entity.Property(e => e.GeneralRuleName)
          .IsRequired()
          .HasMaxLength(512)
          .IsUnicode(false);

        entity.Property(e => e.GeneralRuleStartAnchor)
          .HasMaxLength(1024)
          .IsUnicode(false);
      });

      modelBuilder.Entity<MessageRule>(entity =>
      {
        entity.Property(c => c.MessageRuleId).ValueGeneratedOnAdd();

        entity.HasOne(d => d.GeneralRule)
          .WithMany(p => p.MessageRule)
          .HasForeignKey(d => d.GeneralRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_MessageRule_GeneralRule");
      });

      modelBuilder.Entity<NormalizedLog>(entity =>
      {
        entity.Property(c => c.NormalizedLogId).ValueGeneratedOnAdd();
        entity.HasKey(c => c.NormalizedLogId).ForSqlServerIsClustered(false);

        entity.ForSqlServerHasIndex(e => e.Timestamp).ForSqlServerIsClustered().IsUnique(false);

        entity.Property(e => e.Message)
          .IsRequired()
          .HasMaxLength(2048)
          .IsUnicode(false);

        entity.HasOne(e => e.SeverityLevel)
          .WithMany(sl => sl.NormalizedLogs)
          .HasForeignKey(e => e.SeverityLevelId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_NormalizedLog_SeverityLevel");

        entity.Property(e => e.Timestamp).HasColumnType("datetime");

        entity.HasOne(d => d.Component)
          .WithMany(p => p.NormalizedLog)
          .HasForeignKey(d => d.ComponentId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_NormalizedLog_Component");

        entity.HasOne(d => d.RawLog)
          .WithMany(p => p.NormalizedLog)
          .HasForeignKey(d => d.RawLogId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_NormalizedLog_RawLog");
      });

      modelBuilder.Entity<ParsingRules>(entity =>
      {
        entity.Property(c => c.ParsingRulesId).ValueGeneratedOnAdd();

        entity.HasOne(d => d.DateTimeRule)
          .WithMany(p => p.ParsingRules)
          .HasForeignKey(d => d.DateTimeRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_ParsingRules_DateTimeRule");

        entity.HasOne(d => d.MessageRule)
          .WithMany(p => p.ParsingRules)
          .HasForeignKey(d => d.MessageRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_ParsingRules_MessageRule");

        entity.HasOne(d => d.SeverityLevelRule)
          .WithMany(p => p.ParsingRules)
          .HasForeignKey(d => d.SeverityLevelRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_ParsingRules_SeverityLevelRule");
      });

      modelBuilder.Entity<RawLog>(entity =>
      {
        entity.Property(c => c.RawLogId).ValueGeneratedOnAdd();

        entity.Property(e => e.Message)
          .IsRequired()
          .HasMaxLength(4096)
          .IsUnicode(false);

        entity.HasOne(d => d.Component)
          .WithMany(p => p.RawLog)
          .HasForeignKey(d => d.ComponentId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_RawLog_Component");
      });

      modelBuilder.Entity<SeverityLevelRule>(entity =>
      {
        entity.Property(c => c.SeverityLevelRuleId).ValueGeneratedOnAdd();

        entity.Property(e => e.Debug)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.Property(e => e.Error)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.Property(e => e.Fatal)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.Property(e => e.Info)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.Property(e => e.Trace)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.Property(e => e.Warning)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);

        entity.HasOne(d => d.GeneralRule)
          .WithMany(p => p.SeverityLevelRule)
          .HasForeignKey(d => d.GeneralRuleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_SeverityLevelRule_GeneralRule");
      });

      modelBuilder.Entity<SeverityLevel>(entity =>
      {
        entity.Property(e => e.SeverityLevelId).ValueGeneratedOnAdd();

        entity.HasData(
          new SeverityLevel {SeverityLevelId = (int) Processor.Model.SeverityLevel.Debug, SeverityLevelValue = "Debug"},
          new SeverityLevel {SeverityLevelId = (int) Processor.Model.SeverityLevel.Trace, SeverityLevelValue = "Trace"},
          new SeverityLevel {SeverityLevelId = (int) Processor.Model.SeverityLevel.Info, SeverityLevelValue = "Info"},
          new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Warning, SeverityLevelValue = "Warning"},
          new SeverityLevel {SeverityLevelId = (int) Processor.Model.SeverityLevel.Error, SeverityLevelValue = "Error"},
          new SeverityLevel {SeverityLevelId = (int) Processor.Model.SeverityLevel.Fatal, SeverityLevelValue = "Fatal"}
        );
      });

      modelBuilder.Entity<System>(entity =>
      {
        entity.Property(c => c.SystemId).ValueGeneratedOnAdd();

        entity.Property(e => e.SystemDescription)
          .IsRequired()
          .HasMaxLength(2048)
          .IsUnicode(false);

        entity.Property(e => e.SystemName)
          .IsRequired()
          .HasMaxLength(256)
          .IsUnicode(false);
      });

      modelBuilder.Entity<CustomAttributeRuleParsingRules>(entity =>
      {
        entity.HasOne(d => d.ParsingRules)
          .WithMany(d => d.CustomAttributeRuleParsingRules)
          .HasForeignKey(d => d.ParsingRulesId);

        entity.HasOne(d => d.GeneralRule)
          .WithMany(d => d.CustomAttributeRuleParsingRules)
          .HasForeignKey(d => d.GeneralRuleId);

        entity.HasKey(o => new {o.ParsingRulesId, CustomAttributeRuleId = o.GeneralRuleId});
      });
    }
  }
}