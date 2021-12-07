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
using System.Net;
using System.Text;
using CsvHelper;

namespace front.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ExportService<SensorReading> _export;
        public List<SensorReading> myList = new List<SensorReading>();
        public List<int> Label = new List<int>();
        public List<double> Data = new List<double>();

        public IndexModel(ILogger<IndexModel> logger,ExportService<SensorReading> export)
        {
            _logger = logger;
			_export = export;
        }

         public async Task OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
            var request = new System.Net.Http.HttpRequestMessage();
            request.RequestUri = new Uri("http://api:80/api/temperature");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            myList = JsonSerializer.Deserialize<List<SensorReading>>(responseContent);
            Data = myList.Select(item => item.value).ToList();
            Label = Enumerable.Range(1, myList.Count).ToList();
			Console.WriteLine("get");
			_export.ExportCSV(myList,"temperature");
			_export.ExportJSON(myList,"temperature");
            }
        }
    }
}
