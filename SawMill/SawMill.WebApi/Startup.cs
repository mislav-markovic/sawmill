using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SawMill.Data.Models;
using SawMill.Data.Repository;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Impl;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.BackgroundService;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.Alert;
using SawMill.WebApi.ViewModel.AlertGroup;
using SawMill.WebApi.ViewModel.AlertValue;
using SawMill.WebApi.ViewModel.Component;
using SawMill.WebApi.ViewModel.CustomAttributeRule;
using SawMill.WebApi.ViewModel.DateTimeRule;
using SawMill.WebApi.ViewModel.Log;
using SawMill.WebApi.ViewModel.MessageRule;
using SawMill.WebApi.ViewModel.ParsingRules;
using SawMill.WebApi.ViewModel.Reports;
using SawMill.WebApi.ViewModel.SeverityRule;
using SawMill.WebApi.ViewModel.System;
using Alert = SawMill.Processor.Model.Alert;
using AlertValue = SawMill.Processor.Model.AlertValue;
using Component = SawMill.Processor.Model.Component;
using DateTimeRule = SawMill.Processor.Model.DateTimeRule;
using GeneralRule = SawMill.Processor.Model.GeneralRule;
using MessageRule = SawMill.Processor.Model.MessageRule;
using NormalizedLog = SawMill.Processor.Model.NormalizedLog;
using ParsingRules = SawMill.Processor.Model.ParsingRules;
using RawLog = SawMill.Processor.Model.RawLog;
using SystemReport = SawMill.Processor.Model.SystemReport;

namespace SawMill.WebApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // db context
      services.AddScoped<SawMillDbContext>();

      // repository
      services.AddScoped<ISystemRepository, SystemRepository>();
      services.AddScoped<IComponentRepository, ComponentRepository>();
      services.AddScoped<IParsingRulesRepository, ParsingRulesRepository>();
      services.AddScoped<IGeneralRuleRepository, GeneralRuleRepository>();
      services.AddScoped<IMessageRuleRepository, MessageRuleRepository>();
      services.AddScoped<IDateTimeRuleRepository, DateTimeRuleRepository>();
      services.AddScoped<ISeverityRuleRepository, SeverityRuleRepository>();
      services.AddScoped<IRawLogRepository, RawLogRepository>();
      services.AddScoped<INormalizedLogRepository, NormalizedLogRepository>();
      services.AddScoped<IAlertRepository, AlertRepository>();
      services.AddScoped<IAlertValueRepository, AlertValueRepository>();
      services.AddScoped<IAlertGroupRepository, AlertGroupRepository>();
      services.AddScoped<IAlertGroupValueRepository, AlertGroupValueRepository>();
      services.AddScoped<ISystemReportRepository, SystemReportRepository>();
      services.AddScoped<ISettingsRepository, SettingsRepository>();

      // services
      services.AddTransient<ISystemService, SystemService>();
      services.AddTransient<IComponentService, ComponentService>();
      services.AddTransient<IParsingRulesService, ParsingRulesService>();
      services.AddTransient<IMessageRuleService, MessageRuleService>();
      services.AddTransient<IDateTimeRuleService, DateTimeRuleService>();
      services.AddTransient<ISeverityRuleService, SeverityRuleService>();
      services.AddTransient<ICustomAttributeRuleService, CustomAttributeRuleService>();
      services.AddTransient<IParseService, ParseService>();
      services.AddTransient<IRawLogService, RawLogService>();
      services.AddTransient<INormalizedLogService, NormalizedLogService>();
      services.AddTransient<IAlertService, AlertService>();
      services.AddTransient<IAlertValueService, AlertValueService>();
      services.AddTransient<IAlertGroupService, AlertGroupService>();
      services.AddTransient<IAlertGroupValueService, AlertGroupValueService>();
      services.AddTransient<ISettingsService, SettingsService>();

      // presenters
      services.AddTransient<IPresenter<SystemViewModel, Processor.Model.System>, SystemPresenter>();
      services.AddTransient<IPresenter<ComponentViewModel, Component>, ComponentPresenter>();
      services.AddTransient<IPresenter<DateTimeRuleViewModel, DateTimeRule>, DateTimeRulePresenter>();
      services.AddTransient<IPresenter<MessageRuleViewModel, MessageRule>, MessageRulePresenter>();
      services.AddTransient<IPresenter<SeverityRuleViewModel, SeverityRule>, SeverityRulePresenter>();
      services.AddTransient<IPresenter<ParsingRulesViewModel, ParsingRules>, ParsingRulesPresenter>();
      services
        .AddTransient<IPresenter<CustomAttributeRuleViewModel, GeneralRule>, CustomAttributeRulePresenter>();
      services.AddTransient<IPresenter<RawLogViewModel, RawLog>, RawLogPresenter>();
      services.AddTransient<IPresenter<NormalizedLogViewModel, NormalizedLog>, NormalizedLogPresenter>();
      services.AddTransient<IPresenter<AlertViewModel, Alert>, AlertPresenter>();
      services.AddTransient<IPresenter<AlertValueViewModel, AlertValue>, AlertValuePresenter>();
      services.AddTransient<IPresenter<SystemReportViewModel, SystemReport>, SystemReportPresenter>();
      services.AddTransient<IPresenter<AlertGroupViewModel, Processor.Model.AlertGroup>, AlertGroupPresenter>();

      // other
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddCors();

      // background services
      services.AddScoped<IAnalyzerJobService, AnalyzerJobService>();
      services.AddHostedService<ScopedServiceProvider>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      {
        app.UseHsts();
      }

      app.UseCors(options => options.WithOrigins("http://localhost:8080", "http://localhost:8081").AllowAnyHeader()
        .AllowAnyMethod());
      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}