﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SawMill.Data.Models;

namespace SawMill.Data.Migrations
{
    [DbContext(typeof(SawMillDbContext))]
    [Migration("20200203200246_RemoveCorrelation")]
    partial class RemoveCorrelation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SawMill.Data.Models.Alert", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComponentId");

                    b.Property<string>("Description");

                    b.Property<int>("GeneralRuleId");

                    b.Property<bool>("HasConstantValue");

                    b.Property<string>("Name");

                    b.Property<int>("Threshold");

                    b.Property<long>("Timespan");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("AlertId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("GeneralRuleId");

                    b.ToTable("Alert");
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroup", b =>
                {
                    b.Property<int>("AlertGroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CorrelationWindow");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("SystemId");

                    b.HasKey("AlertGroupId");

                    b.HasIndex("SystemId");

                    b.ToTable("AlertGroup");
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroupAlert", b =>
                {
                    b.Property<int>("AlertId");

                    b.Property<int>("AlertGroupId");

                    b.Property<bool>("Not");

                    b.Property<int>("Position");

                    b.HasKey("AlertId", "AlertGroupId");

                    b.HasIndex("AlertGroupId");

                    b.ToTable("AlertGroupAlert");
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroupValue", b =>
                {
                    b.Property<int>("AlertGroupValueId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlertGroupId");

                    b.Property<DateTime>("TimespanEnd")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TimespanStart")
                        .HasColumnType("datetime");

                    b.HasKey("AlertGroupValueId");

                    b.HasIndex("AlertGroupId");

                    b.ToTable("AlertGroupValue");
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertValue", b =>
                {
                    b.Property<int>("AlertValueId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlertGroupValueId");

                    b.Property<int>("AlertId");

                    b.Property<string>("ConstantValue");

                    b.Property<DateTime>("TimespanEnd")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("TimespanStart")
                        .HasColumnType("datetime");

                    b.HasKey("AlertValueId");

                    b.HasIndex("AlertGroupValueId");

                    b.HasIndex("AlertId");

                    b.ToTable("AlertValue");
                });

            modelBuilder.Entity("SawMill.Data.Models.Component", b =>
                {
                    b.Property<int>("ComponentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ComponentDescription");

                    b.Property<string>("ComponentName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int?>("ParsingRulesId");

                    b.Property<int?>("SystemId");

                    b.HasKey("ComponentId");

                    b.HasIndex("ComponentName")
                        .IsUnique();

                    b.HasIndex("ParsingRulesId");

                    b.HasIndex("SystemId");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("SawMill.Data.Models.ComponentReport", b =>
                {
                    b.Property<int>("ComponentReportId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComponentId");

                    b.Property<int>("SystemReportId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("ComponentReportId");

                    b.HasIndex("ComponentId");

                    b.HasIndex("SystemReportId");

                    b.ToTable("ComponentReport");
                });

            modelBuilder.Entity("SawMill.Data.Models.ComponentReportAlert", b =>
                {
                    b.Property<int>("ComponentReportAlertId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlertId");

                    b.Property<int>("ComponentReportId");

                    b.Property<int>("Count");

                    b.HasKey("ComponentReportAlertId");

                    b.HasIndex("AlertId");

                    b.HasIndex("ComponentReportId");

                    b.ToTable("ComponentReportAlert");
                });

            modelBuilder.Entity("SawMill.Data.Models.CustomAttributeRuleParsingRules", b =>
                {
                    b.Property<int>("ParsingRulesId");

                    b.Property<int>("GeneralRuleId");

                    b.HasKey("ParsingRulesId", "GeneralRuleId");

                    b.HasIndex("GeneralRuleId");

                    b.ToTable("CustomAttributeRuleParsingRules");
                });

            modelBuilder.Entity("SawMill.Data.Models.CustomAttributeValue", b =>
                {
                    b.Property<int>("CustomAttributeValueId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GeneralRuleId");

                    b.Property<int>("NormalizedLogId");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false);

                    b.HasKey("CustomAttributeValueId");

                    b.HasIndex("GeneralRuleId");

                    b.HasIndex("NormalizedLogId");

                    b.ToTable("CustomAttributeValue");
                });

            modelBuilder.Entity("SawMill.Data.Models.DateTimeRule", b =>
                {
                    b.Property<int>("DateTimeRuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateTimeFormat")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("GeneralRuleId");

                    b.HasKey("DateTimeRuleId");

                    b.HasIndex("GeneralRuleId");

                    b.ToTable("DateTimeRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.GeneralRule", b =>
                {
                    b.Property<int>("GeneralRuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GeneralRuleDescription")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .IsUnicode(false);

                    b.Property<string>("GeneralRuleEndAnchor")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.Property<string>("GeneralRuleMatcher")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false);

                    b.Property<string>("GeneralRuleName")
                        .IsRequired()
                        .HasMaxLength(512)
                        .IsUnicode(false);

                    b.Property<string>("GeneralRuleStartAnchor")
                        .HasMaxLength(1024)
                        .IsUnicode(false);

                    b.HasKey("GeneralRuleId");

                    b.ToTable("GeneralRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.MessageRule", b =>
                {
                    b.Property<int>("MessageRuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GeneralRuleId");

                    b.Property<int?>("MaxLength");

                    b.HasKey("MessageRuleId");

                    b.HasIndex("GeneralRuleId");

                    b.ToTable("MessageRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.NormalizedLog", b =>
                {
                    b.Property<int>("NormalizedLogId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComponentId");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false);

                    b.Property<int>("RawLogId");

                    b.Property<int>("SeverityLevelId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("NormalizedLogId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("ComponentId");

                    b.HasIndex("RawLogId");

                    b.HasIndex("SeverityLevelId");

                    b.HasIndex("Timestamp")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("NormalizedLog");
                });

            modelBuilder.Entity("SawMill.Data.Models.ParsingRules", b =>
                {
                    b.Property<int>("ParsingRulesId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DateTimeRuleId");

                    b.Property<int>("MessageRuleId");

                    b.Property<int>("SeverityLevelRuleId");

                    b.HasKey("ParsingRulesId");

                    b.HasIndex("DateTimeRuleId");

                    b.HasIndex("MessageRuleId");

                    b.HasIndex("SeverityLevelRuleId");

                    b.ToTable("ParsingRules");
                });

            modelBuilder.Entity("SawMill.Data.Models.RawLog", b =>
                {
                    b.Property<int>("RawLogId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ComponentId");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .IsUnicode(false);

                    b.HasKey("RawLogId");

                    b.HasIndex("ComponentId");

                    b.ToTable("RawLog");
                });

            modelBuilder.Entity("SawMill.Data.Models.Settings", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Frequency");

                    b.Property<string>("Name");

                    b.HasKey("SettingsId");

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            SettingsId = 1,
                            Frequency = 60L,
                            Name = "Analyzer Frequency"
                        });
                });

            modelBuilder.Entity("SawMill.Data.Models.SeverityLevel", b =>
                {
                    b.Property<int>("SeverityLevelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SeverityLevelValue");

                    b.HasKey("SeverityLevelId");

                    b.ToTable("SeverityLevel");

                    b.HasData(
                        new
                        {
                            SeverityLevelId = 1,
                            SeverityLevelValue = "Debug"
                        },
                        new
                        {
                            SeverityLevelId = 2,
                            SeverityLevelValue = "Trace"
                        },
                        new
                        {
                            SeverityLevelId = 3,
                            SeverityLevelValue = "Info"
                        },
                        new
                        {
                            SeverityLevelId = 4,
                            SeverityLevelValue = "Warning"
                        },
                        new
                        {
                            SeverityLevelId = 5,
                            SeverityLevelValue = "Error"
                        },
                        new
                        {
                            SeverityLevelId = 6,
                            SeverityLevelValue = "Fatal"
                        });
                });

            modelBuilder.Entity("SawMill.Data.Models.SeverityLevelRule", b =>
                {
                    b.Property<int>("SeverityLevelRuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Debug")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Error")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Fatal")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("GeneralRuleId");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Trace")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Warning")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("SeverityLevelRuleId");

                    b.HasIndex("GeneralRuleId");

                    b.ToTable("SeverityLevelRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.System", b =>
                {
                    b.Property<int>("SystemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SystemDescription")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false);

                    b.Property<string>("SystemName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("SystemId");

                    b.ToTable("System");
                });

            modelBuilder.Entity("SawMill.Data.Models.SystemReport", b =>
                {
                    b.Property<int>("SystemReportId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SystemId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("SystemReportId");

                    b.HasIndex("SystemId");

                    b.ToTable("SystemReport");
                });

            modelBuilder.Entity("SawMill.Data.Models.SystemReportAlertGroup", b =>
                {
                    b.Property<int>("SystemReportAlertGroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlertGroupId");

                    b.Property<int>("Count");

                    b.Property<int>("SystemReportId");

                    b.HasKey("SystemReportAlertGroupId");

                    b.HasIndex("AlertGroupId");

                    b.HasIndex("SystemReportId");

                    b.ToTable("SystemReportAlertGroup");
                });

            modelBuilder.Entity("SawMill.Data.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<string>("Name");

                    b.HasKey("UserId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Category = "guest",
                            Name = "User A"
                        },
                        new
                        {
                            UserId = 2,
                            Category = "guest",
                            Name = "User B"
                        },
                        new
                        {
                            UserId = 3,
                            Category = "admin",
                            Name = "User C"
                        });
                });

            modelBuilder.Entity("SawMill.Data.Models.Alert", b =>
                {
                    b.HasOne("SawMill.Data.Models.Component", "Component")
                        .WithMany("Alerts")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_Alert_Component")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("Alerts")
                        .HasForeignKey("GeneralRuleId")
                        .HasConstraintName("FK_Alert_GeneralRule")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroup", b =>
                {
                    b.HasOne("SawMill.Data.Models.System", "System")
                        .WithMany("AlertGroup")
                        .HasForeignKey("SystemId")
                        .HasConstraintName("FK_AlertGroup_System")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroupAlert", b =>
                {
                    b.HasOne("SawMill.Data.Models.AlertGroup", "AlertGroup")
                        .WithMany("AlertGroupAlert")
                        .HasForeignKey("AlertGroupId")
                        .HasConstraintName("FK_AlertGroup_AlertGroupAlert");

                    b.HasOne("SawMill.Data.Models.Alert", "Alert")
                        .WithMany("AlertGroupAlerts")
                        .HasForeignKey("AlertId")
                        .HasConstraintName("FK_Alert_AlertGroupAlert");
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertGroupValue", b =>
                {
                    b.HasOne("SawMill.Data.Models.AlertGroup", "AlertGroup")
                        .WithMany("AlertGroupValues")
                        .HasForeignKey("AlertGroupId")
                        .HasConstraintName("FK_AlertGroup_AlertGroupValue")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.AlertValue", b =>
                {
                    b.HasOne("SawMill.Data.Models.AlertGroupValue", "AlertGroupValue")
                        .WithMany("AlertValues")
                        .HasForeignKey("AlertGroupValueId")
                        .HasConstraintName("FK_AlertValue_AlertGroupValue");

                    b.HasOne("SawMill.Data.Models.Alert", "Alert")
                        .WithMany("AlertValues")
                        .HasForeignKey("AlertId")
                        .HasConstraintName("FK_AlertValue_Alert");
                });

            modelBuilder.Entity("SawMill.Data.Models.Component", b =>
                {
                    b.HasOne("SawMill.Data.Models.ParsingRules", "ParsingRules")
                        .WithMany("Components")
                        .HasForeignKey("ParsingRulesId")
                        .HasConstraintName("FK_Component_ParsingRules");

                    b.HasOne("SawMill.Data.Models.System", "System")
                        .WithMany("Component")
                        .HasForeignKey("SystemId")
                        .HasConstraintName("FK_Component_System");
                });

            modelBuilder.Entity("SawMill.Data.Models.ComponentReport", b =>
                {
                    b.HasOne("SawMill.Data.Models.Component", "Component")
                        .WithMany("ComponentReports")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_Component_ComponentReports");

                    b.HasOne("SawMill.Data.Models.SystemReport", "SystemReport")
                        .WithMany("ComponentReports")
                        .HasForeignKey("SystemReportId")
                        .HasConstraintName("FK_SystemReport_ComponentReport")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.ComponentReportAlert", b =>
                {
                    b.HasOne("SawMill.Data.Models.Alert", "Alert")
                        .WithMany("ComponentReportAlerts")
                        .HasForeignKey("AlertId")
                        .HasConstraintName("FK_Alert_ComponentReportAlert");

                    b.HasOne("SawMill.Data.Models.ComponentReport", "ComponentReport")
                        .WithMany("ComponentReportAlert")
                        .HasForeignKey("ComponentReportId")
                        .HasConstraintName("FK_ComponentReport_ComponentReportAlert");
                });

            modelBuilder.Entity("SawMill.Data.Models.CustomAttributeRuleParsingRules", b =>
                {
                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("CustomAttributeRuleParsingRules")
                        .HasForeignKey("GeneralRuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SawMill.Data.Models.ParsingRules", "ParsingRules")
                        .WithMany("CustomAttributeRuleParsingRules")
                        .HasForeignKey("ParsingRulesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.CustomAttributeValue", b =>
                {
                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("CustomAttributeValues")
                        .HasForeignKey("GeneralRuleId")
                        .HasConstraintName("FK_CustomAttributeValue_GeneralRule");

                    b.HasOne("SawMill.Data.Models.NormalizedLog", "NormalizedLog")
                        .WithMany("CustomAttributeValues")
                        .HasForeignKey("NormalizedLogId")
                        .HasConstraintName("FK_CustomAttributeValue_NormalizedLog")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.DateTimeRule", b =>
                {
                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("DateTimeRule")
                        .HasForeignKey("GeneralRuleId")
                        .HasConstraintName("FK_DateTimeRule_GeneralRule")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SawMill.Data.Models.MessageRule", b =>
                {
                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("MessageRule")
                        .HasForeignKey("GeneralRuleId")
                        .HasConstraintName("FK_MessageRule_GeneralRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.NormalizedLog", b =>
                {
                    b.HasOne("SawMill.Data.Models.Component", "Component")
                        .WithMany("NormalizedLog")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_NormalizedLog_Component");

                    b.HasOne("SawMill.Data.Models.RawLog", "RawLog")
                        .WithMany("NormalizedLog")
                        .HasForeignKey("RawLogId")
                        .HasConstraintName("FK_NormalizedLog_RawLog");

                    b.HasOne("SawMill.Data.Models.SeverityLevel", "SeverityLevel")
                        .WithMany("NormalizedLogs")
                        .HasForeignKey("SeverityLevelId")
                        .HasConstraintName("FK_NormalizedLog_SeverityLevel");
                });

            modelBuilder.Entity("SawMill.Data.Models.ParsingRules", b =>
                {
                    b.HasOne("SawMill.Data.Models.DateTimeRule", "DateTimeRule")
                        .WithMany("ParsingRules")
                        .HasForeignKey("DateTimeRuleId")
                        .HasConstraintName("FK_ParsingRules_DateTimeRule");

                    b.HasOne("SawMill.Data.Models.MessageRule", "MessageRule")
                        .WithMany("ParsingRules")
                        .HasForeignKey("MessageRuleId")
                        .HasConstraintName("FK_ParsingRules_MessageRule");

                    b.HasOne("SawMill.Data.Models.SeverityLevelRule", "SeverityLevelRule")
                        .WithMany("ParsingRules")
                        .HasForeignKey("SeverityLevelRuleId")
                        .HasConstraintName("FK_ParsingRules_SeverityLevelRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.RawLog", b =>
                {
                    b.HasOne("SawMill.Data.Models.Component", "Component")
                        .WithMany("RawLog")
                        .HasForeignKey("ComponentId")
                        .HasConstraintName("FK_RawLog_Component");
                });

            modelBuilder.Entity("SawMill.Data.Models.SeverityLevelRule", b =>
                {
                    b.HasOne("SawMill.Data.Models.GeneralRule", "GeneralRule")
                        .WithMany("SeverityLevelRule")
                        .HasForeignKey("GeneralRuleId")
                        .HasConstraintName("FK_SeverityLevelRule_GeneralRule");
                });

            modelBuilder.Entity("SawMill.Data.Models.SystemReport", b =>
                {
                    b.HasOne("SawMill.Data.Models.System", "System")
                        .WithMany("SystemReports")
                        .HasForeignKey("SystemId")
                        .HasConstraintName("FK_System_SystemReport");
                });

            modelBuilder.Entity("SawMill.Data.Models.SystemReportAlertGroup", b =>
                {
                    b.HasOne("SawMill.Data.Models.AlertGroup", "AlertGroup")
                        .WithMany("SystemReportAlertGroups")
                        .HasForeignKey("AlertGroupId")
                        .HasConstraintName("FK_AlertGroup_SystemReportAlertGroup")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SawMill.Data.Models.SystemReport", "SystemReport")
                        .WithMany("SystemReportAlertGroup")
                        .HasForeignKey("SystemReportId")
                        .HasConstraintName("FK_SystemReport_SystemReportAlertGroup");
                });
#pragma warning restore 612, 618
        }
    }
}
