﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using front.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace front.Pages
{
    public class Bears : PageModel
    {
        public List<SensorReading> myList = new List<SensorReading>();
        public List<int> Label = new List<int>();
        public List<double> Data = new List<double>();
        
        public async Task OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://api:80/api/bears");
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                myList = JsonSerializer.Deserialize<List<SensorReading>>(responseContent);
                Data = myList.Select(item => item.value).ToList();
                Label = Enumerable.Range(1, myList.Count).ToList();
                Console.WriteLine("get");
                //_export.ExportCSV(myList,"mojanazwa");
                //_export.ExportJSON(myList,"mojanazwa");
            }
        }
    }
}