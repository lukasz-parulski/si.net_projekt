using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using front.Models;
using front.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace front.Pages
{
    public class Bears : PageModel
    {
        private readonly ExportService<SensorReading> _export;
        public List<SensorReading> myList = new List<SensorReading>();
        public List<int> Label = new List<int>();
        public List<double> Data = new List<double>();

        public Bears(ExportService<SensorReading> export)
        {
            _export = export;
        }
        
        [HttpGet("{myId}")]
        public async Task OnGet([FromQuery]int myId)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://api:80/api/bears");
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
                _export.ExportCSV(myList,"bears");
                _export.ExportJSON(myList,"bears");
            }
        }
        [HttpPost("{myId}")]
        public async Task OnPost([FromQuery]int myId)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://api:80/api/bears");
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
                _export.ExportCSV(myList,"bears");
                _export.ExportJSON(myList,"bears");
            }
        }
    }
}