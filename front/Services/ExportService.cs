using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using CsvHelper;

namespace front.Services
{
    public class ExportService<T> where T : class
    {
        public void ExportCSV(List<T> list,string name)
        {
            Console.WriteLine("csv: " + name);
            string fileName = name + ".csv";
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var fs = new FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            var streamWriter = new StreamWriter(fs);
            var csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(list);
            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }
        public void ExportJSON(List<T> list,string name)
        {
            Console.WriteLine("json: " + name);
            string jsonString = JsonSerializer.Serialize(list);
            string fileName = name + ".txt";
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var fs = new FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(jsonString);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}