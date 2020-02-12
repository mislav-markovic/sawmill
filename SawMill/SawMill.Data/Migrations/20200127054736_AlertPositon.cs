using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SawMill.Data.Migrations
{
    public partial class AlertPositon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralRule",
                columns: table => new
                {
                    GeneralRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GeneralRuleName = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    GeneralRuleDescription = table.Column<string>(unicode: false, maxLength: 4096, nullable: false),
                    GeneralRuleMatcher = table.Column<string>(unicode: false, maxLength: 2048, nullable: false),
                    GeneralRuleStartAnchor = table.Column<string>(unicode: false, maxLength: 1024, nullable: true),
                    GeneralRuleEndAnchor = table.Column<string>(unicode: false, maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralRule", x => x.GeneralRuleId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Frequency = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateTable(
                name: "SeverityLevel",
                columns: table => new
                {
                    SeverityLevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeverityLevelValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeverityLevel", x => x.SeverityLevelId);
                });

            migrationBuilder.CreateTable(
                name: "System",
                columns: table => new
                {
                    SystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemName = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    SystemDescription = table.Column<string>(unicode: false, maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System", x => x.SystemId);
                });

            migrationBuilder.CreateTable(
                name: "DateTimeRule",
                columns: table => new
                {
                    DateTimeRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GeneralRuleId = table.Column<int>(nullable: false),
                    DateTimeFormat = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateTimeRule", x => x.DateTimeRuleId);
                    table.ForeignKey(
                        name: "FK_DateTimeRule_GeneralRule",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageRule",
                columns: table => new
                {
                    MessageRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaxLength = table.Column<int>(nullable: true),
                    GeneralRuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRule", x => x.MessageRuleId);
                    table.ForeignKey(
                        name: "FK_MessageRule_GeneralRule",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeverityLevelRule",
                columns: table => new
                {
                    SeverityLevelRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GeneralRuleId = table.Column<int>(nullable: false),
                    Trace = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Debug = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Info = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Warning = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Error = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Fatal = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeverityLevelRule", x => x.SeverityLevelRuleId);
                    table.ForeignKey(
                        name: "FK_SeverityLevelRule_GeneralRule",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertGroup",
                columns: table => new
                {
                    AlertGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CorrelationWindow = table.Column<long>(nullable: false),
                    SystemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertGroup", x => x.AlertGroupId);
                    table.ForeignKey(
                        name: "FK_AlertGroup_System",
                        column: x => x.SystemId,
                        principalTable: "System",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Correlation",
                columns: table => new
                {
                    CorrelationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemId = table.Column<int>(nullable: false),
                    CauseValue = table.Column<string>(nullable: true),
                    EffectValue = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    IsUserCorrelation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Correlation", x => x.CorrelationId);
                    table.ForeignKey(
                        name: "FK_Correlation_System",
                        column: x => x.SystemId,
                        principalTable: "System",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemReport",
                columns: table => new
                {
                    SystemReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemReport", x => x.SystemReportId);
                    table.ForeignKey(
                        name: "FK_System_SystemReport",
                        column: x => x.SystemId,
                        principalTable: "System",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParsingRules",
                columns: table => new
                {
                    ParsingRulesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTimeRuleId = table.Column<int>(nullable: false),
                    MessageRuleId = table.Column<int>(nullable: false),
                    SeverityLevelRuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsingRules", x => x.ParsingRulesId);
                    table.ForeignKey(
                        name: "FK_ParsingRules_DateTimeRule",
                        column: x => x.DateTimeRuleId,
                        principalTable: "DateTimeRule",
                        principalColumn: "DateTimeRuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParsingRules_MessageRule",
                        column: x => x.MessageRuleId,
                        principalTable: "MessageRule",
                        principalColumn: "MessageRuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParsingRules_SeverityLevelRule",
                        column: x => x.SeverityLevelRuleId,
                        principalTable: "SeverityLevelRule",
                        principalColumn: "SeverityLevelRuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertGroupValue",
                columns: table => new
                {
                    AlertGroupValueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlertGroupId = table.Column<int>(nullable: false),
                    TimespanStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    TimespanEnd = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertGroupValue", x => x.AlertGroupValueId);
                    table.ForeignKey(
                        name: "FK_AlertGroup_AlertGroupValue",
                        column: x => x.AlertGroupId,
                        principalTable: "AlertGroup",
                        principalColumn: "AlertGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemReportAlertGroup",
                columns: table => new
                {
                    SystemReportAlertGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SystemReportId = table.Column<int>(nullable: false),
                    AlertGroupId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemReportAlertGroup", x => x.SystemReportAlertGroupId);
                    table.ForeignKey(
                        name: "FK_AlertGroup_SystemReportAlertGroup",
                        column: x => x.AlertGroupId,
                        principalTable: "AlertGroup",
                        principalColumn: "AlertGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemReport_SystemReportAlertGroup",
                        column: x => x.SystemReportId,
                        principalTable: "SystemReport",
                        principalColumn: "SystemReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComponentName = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    ComponentDescription = table.Column<string>(nullable: true),
                    SystemId = table.Column<int>(nullable: true),
                    ParsingRulesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Component_ParsingRules",
                        column: x => x.ParsingRulesId,
                        principalTable: "ParsingRules",
                        principalColumn: "ParsingRulesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Component_System",
                        column: x => x.SystemId,
                        principalTable: "System",
                        principalColumn: "SystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomAttributeRuleParsingRules",
                columns: table => new
                {
                    ParsingRulesId = table.Column<int>(nullable: false),
                    GeneralRuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributeRuleParsingRules", x => new { x.ParsingRulesId, x.GeneralRuleId });
                    table.ForeignKey(
                        name: "FK_CustomAttributeRuleParsingRules_GeneralRule_GeneralRuleId",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomAttributeRuleParsingRules_ParsingRules_ParsingRulesId",
                        column: x => x.ParsingRulesId,
                        principalTable: "ParsingRules",
                        principalColumn: "ParsingRulesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Timespan = table.Column<long>(nullable: false),
                    Threshold = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: false),
                    GeneralRuleId = table.Column<int>(nullable: false),
                    HasConstantValue = table.Column<bool>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    AlertGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alert_AlertGroup",
                        column: x => x.AlertGroupId,
                        principalTable: "AlertGroup",
                        principalColumn: "AlertGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alert_Component",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alert_GeneralRule",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentReport",
                columns: table => new
                {
                    ComponentReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComponentId = table.Column<int>(nullable: false),
                    SystemReportId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentReport", x => x.ComponentReportId);
                    table.ForeignKey(
                        name: "FK_Component_ComponentReports",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemReport_ComponentReport",
                        column: x => x.SystemReportId,
                        principalTable: "SystemReport",
                        principalColumn: "SystemReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawLog",
                columns: table => new
                {
                    RawLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(unicode: false, maxLength: 4096, nullable: false),
                    ComponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawLog", x => x.RawLogId);
                    table.ForeignKey(
                        name: "FK_RawLog_Component",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertValue",
                columns: table => new
                {
                    AlertValueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlertId = table.Column<int>(nullable: false),
                    ConstantValue = table.Column<string>(nullable: true),
                    TimespanStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    TimespanEnd = table.Column<DateTime>(type: "datetime", nullable: false),
                    AlertGroupValueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertValue", x => x.AlertValueId);
                    table.ForeignKey(
                        name: "FK_AlertValue_AlertGroupValue",
                        column: x => x.AlertGroupValueId,
                        principalTable: "AlertGroupValue",
                        principalColumn: "AlertGroupValueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertValue_Alert",
                        column: x => x.AlertId,
                        principalTable: "Alert",
                        principalColumn: "AlertId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComponentReportAlert",
                columns: table => new
                {
                    ComponentReportAlertId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlertId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ComponentReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentReportAlert", x => x.ComponentReportAlertId);
                    table.ForeignKey(
                        name: "FK_Alert_ComponentReportAlert",
                        column: x => x.AlertId,
                        principalTable: "Alert",
                        principalColumn: "AlertId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentReport_ComponentReportAlert",
                        column: x => x.ComponentReportId,
                        principalTable: "ComponentReport",
                        principalColumn: "ComponentReportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NormalizedLog",
                columns: table => new
                {
                    NormalizedLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RawLogId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(unicode: false, maxLength: 2048, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    SeverityLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NormalizedLog", x => x.NormalizedLogId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_NormalizedLog_Component",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NormalizedLog_RawLog",
                        column: x => x.RawLogId,
                        principalTable: "RawLog",
                        principalColumn: "RawLogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NormalizedLog_SeverityLevel",
                        column: x => x.SeverityLevelId,
                        principalTable: "SeverityLevel",
                        principalColumn: "SeverityLevelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomAttributeValue",
                columns: table => new
                {
                    CustomAttributeValueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(unicode: false, maxLength: 2048, nullable: false),
                    NormalizedLogId = table.Column<int>(nullable: false),
                    GeneralRuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomAttributeValue", x => x.CustomAttributeValueId);
                    table.ForeignKey(
                        name: "FK_CustomAttributeValue_GeneralRule",
                        column: x => x.GeneralRuleId,
                        principalTable: "GeneralRule",
                        principalColumn: "GeneralRuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomAttributeValue_NormalizedLog",
                        column: x => x.NormalizedLogId,
                        principalTable: "NormalizedLog",
                        principalColumn: "NormalizedLogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "SettingsId", "Frequency", "Name" },
                values: new object[] { 1, 60L, "Analyzer Frequency" });

            migrationBuilder.InsertData(
                table: "SeverityLevel",
                columns: new[] { "SeverityLevelId", "SeverityLevelValue" },
                values: new object[,]
                {
                    { 1, "Debug" },
                    { 2, "Trace" },
                    { 3, "Info" },
                    { 4, "Warning" },
                    { 5, "Error" },
                    { 6, "Fatal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertGroupId",
                table: "Alert",
                column: "AlertGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_ComponentId",
                table: "Alert",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_GeneralRuleId",
                table: "Alert",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertGroup_SystemId",
                table: "AlertGroup",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertGroupValue_AlertGroupId",
                table: "AlertGroupValue",
                column: "AlertGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertValue_AlertGroupValueId",
                table: "AlertValue",
                column: "AlertGroupValueId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertValue_AlertId",
                table: "AlertValue",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_ComponentName",
                table: "Component",
                column: "ComponentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Component_ParsingRulesId",
                table: "Component",
                column: "ParsingRulesId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_SystemId",
                table: "Component",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReport_ComponentId",
                table: "ComponentReport",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReport_SystemReportId",
                table: "ComponentReport",
                column: "SystemReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReportAlert_AlertId",
                table: "ComponentReportAlert",
                column: "AlertId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentReportAlert_ComponentReportId",
                table: "ComponentReportAlert",
                column: "ComponentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Correlation_SystemId",
                table: "Correlation",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributeRuleParsingRules_GeneralRuleId",
                table: "CustomAttributeRuleParsingRules",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributeValue_GeneralRuleId",
                table: "CustomAttributeValue",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomAttributeValue_NormalizedLogId",
                table: "CustomAttributeValue",
                column: "NormalizedLogId");

            migrationBuilder.CreateIndex(
                name: "IX_DateTimeRule_GeneralRuleId",
                table: "DateTimeRule",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRule_GeneralRuleId",
                table: "MessageRule",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalizedLog_ComponentId",
                table: "NormalizedLog",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalizedLog_RawLogId",
                table: "NormalizedLog",
                column: "RawLogId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalizedLog_SeverityLevelId",
                table: "NormalizedLog",
                column: "SeverityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_NormalizedLog_Timestamp",
                table: "NormalizedLog",
                column: "Timestamp")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_ParsingRules_DateTimeRuleId",
                table: "ParsingRules",
                column: "DateTimeRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ParsingRules_MessageRuleId",
                table: "ParsingRules",
                column: "MessageRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ParsingRules_SeverityLevelRuleId",
                table: "ParsingRules",
                column: "SeverityLevelRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RawLog_ComponentId",
                table: "RawLog",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SeverityLevelRule_GeneralRuleId",
                table: "SeverityLevelRule",
                column: "GeneralRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemReport_SystemId",
                table: "SystemReport",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemReportAlertGroup_AlertGroupId",
                table: "SystemReportAlertGroup",
                column: "AlertGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemReportAlertGroup_SystemReportId",
                table: "SystemReportAlertGroup",
                column: "SystemReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertValue");

            migrationBuilder.DropTable(
                name: "ComponentReportAlert");

            migrationBuilder.DropTable(
                name: "Correlation");

            migrationBuilder.DropTable(
                name: "CustomAttributeRuleParsingRules");

            migrationBuilder.DropTable(
                name: "CustomAttributeValue");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SystemReportAlertGroup");

            migrationBuilder.DropTable(
                name: "AlertGroupValue");

            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropTable(
                name: "ComponentReport");

            migrationBuilder.DropTable(
                name: "NormalizedLog");

            migrationBuilder.DropTable(
                name: "AlertGroup");

            migrationBuilder.DropTable(
                name: "SystemReport");

            migrationBuilder.DropTable(
                name: "RawLog");

            migrationBuilder.DropTable(
                name: "SeverityLevel");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "ParsingRules");

            migrationBuilder.DropTable(
                name: "System");

            migrationBuilder.DropTable(
                name: "DateTimeRule");

            migrationBuilder.DropTable(
                name: "MessageRule");

            migrationBuilder.DropTable(
                name: "SeverityLevelRule");

            migrationBuilder.DropTable(
                name: "GeneralRule");
        }
    }
}
