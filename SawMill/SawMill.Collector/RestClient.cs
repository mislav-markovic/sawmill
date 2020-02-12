using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace SawMill.Collector
{
  public class RestClient
  {
    private readonly string _baseUrl;

    public RestClient(string baseUrl)
    {
      _baseUrl = baseUrl;
    }

    public void SendLog(IEnumerable<string> logLines, string componentName)
    {
      var restClient = new RestSharp.RestClient(_baseUrl);

      var request =
        new RestRequest("log/raw", Method.POST, DataFormat.Json)
          .AddQueryParameter("componentName", componentName)
          .AddBody(logLines.ToArray());

      var response = restClient.Execute<bool>(request);
    }
  }
}