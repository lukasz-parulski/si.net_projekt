using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using front.Models;
using front.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using CsvHelper;

namespace front.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ExportService<WeatherForecast> _export;
        public List<WeatherForecast> myList = new List<WeatherForecast>();
        public List<int> Label = new List<int>();
        public List<int> Data = new List<int>();
        public List<int> DataF = new List<int>();
        

        public IndexModel(ILogger<IndexModel> logger,ExportService<WeatherForecast> export)
        {
            _logger = logger;
			_export = export;
        }

         public async Task OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
            var request = new System.Net.Http.HttpRequestMessage();
            request.RequestUri = new Uri("http://api:80/WeatherForecast");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            myList = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent);
            Data = myList.Select(item => item.temperatureC).ToList();
			DataF = myList.Select(item => item.temperatureF).ToList();
			Label = Enumerable.Range(1, myList.Count).ToList();
			Console.WriteLine("get");
			_export.ExportCSV(myList,"mojanazwa");
			_export.ExportJSON(myList,"mojanazwa");
            }
        }
    }
}
