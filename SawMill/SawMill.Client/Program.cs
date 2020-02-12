using System;
using System.Threading;
using RestSharp;
using Serilog;

namespace SawMill.Client
{
  class Program
  {
    static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("D:\\test\\client.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

      FailedLogin();
      //FailedLogin2();
      //DropTableQuery();


      Log.CloseAndFlush();
      Console.ReadKey();
    }

    public static void DropTableQuery()
    {

      var restClient = new RestClient("http://localhost:52361/api/test/");

      var category1 = "guest";
      var category2 = "'; drop table [sawmill.db].[dbo].[User] --";
      var category3 = "admin";

      var request = new RestRequest("filter", Method.GET);

      Log.Information($"Querying users with category {category1}");
      request.AddOrUpdateParameter("category", category1, ParameterType.QueryString);
      var response1 = restClient.Execute<bool>(request);

      Log.Information($"Querying users with category {category2}");
      Thread.Sleep(300);
      request.AddOrUpdateParameter("category", category2, ParameterType.QueryString);
      var response2 = restClient.Execute<bool>(request);

      Log.Information($"Querying users with category {category3}");
      request.AddOrUpdateParameter("category", category3, ParameterType.QueryString);
      var response3 = restClient.Execute<bool>(request);
    }

    public static void FailedLogin()
    {
      var restClient = new RestClient("http://localhost:52361/api/test/");

      for (int i = 0; i < 10; i++)
      {
        var request = new RestRequest("login/fail/{userId}", Method.GET);
        request.AddParameter("userId", 1, ParameterType.UrlSegment);
        Thread.Sleep(1100);
        var response = restClient.Execute<bool>(request);
      }
    }
    public static void FailedLogin2()
    {
      var restClient = new RestClient("http://localhost:52361/api/test/");

      for (int i = 0; i < 10; i++)
      {
        var request = new RestRequest("login/fail/{userId}", Method.GET);
        request.AddParameter("userId", i, ParameterType.UrlSegment);
        Thread.Sleep(500);
        var response = restClient.Execute<bool>(request);
      }
    }
  }
}
