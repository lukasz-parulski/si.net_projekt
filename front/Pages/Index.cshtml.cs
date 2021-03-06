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

		[HttpGet("{myId}")]
        public async Task OnGet([FromQuery]int myId)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
            var request = new System.Net.Http.HttpRequestMessage();
            request.RequestUri = new Uri("http://api:80/api/temperature");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            myList = JsonSerializer.Deserialize<List<SensorReading>>(responseContent);
            if (myId>0)
                {
					if(myId==8){
						myList = myList.OrderBy(e=>e.value).ToList();
						Data = myList.Select(item => item.value).ToList();
                    	Label = Enumerable.Range(1, myList.Count).ToList();
					}
					else
					{
                    myList = myList.Where(e=>e.sensorId==myId).ToList();
                    Data = myList.Select(item => item.value).ToList();
                    Label = Enumerable.Range(1, myList.Count).ToList();
					}	
                }
                else
                {
                    Data = myList.Select(item => item.value).ToList();
                    Label = Enumerable.Range(1, myList.Count).ToList();
                }
			Console.WriteLine("get");
			_export.ExportCSV(myList,"temperature");
			_export.ExportJSON(myList,"temperature");
            }
        }
        [HttpPost("{myId}")]
        public async Task OnPost([FromQuery]int myId)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
            var request = new System.Net.Http.HttpRequestMessage();
            request.RequestUri = new Uri("http://api:80/api/temperature");
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            myList = JsonSerializer.Deserialize<List<SensorReading>>(responseContent);
            if (myId>0)
                {
					if(myId==8){
						myList = myList.OrderBy(e=>e.value).ToList();
						Data = myList.Select(item => item.value).ToList();
                    	Label = Enumerable.Range(1, myList.Count).ToList();
					}
					else
					{
                    myList = myList.Where(e=>e.sensorId==myId).ToList();
                    Data = myList.Select(item => item.value).ToList();
                    Label = Enumerable.Range(1, myList.Count).ToList();
					}	
                }
                else
                {
                    Data = myList.Select(item => item.value).ToList();
                    Label = Enumerable.Range(1, myList.Count).ToList();
                }
			Console.WriteLine("get");
			_export.ExportCSV(myList,"temperature");
			_export.ExportJSON(myList,"temperature");
            }
        }
    }
}
