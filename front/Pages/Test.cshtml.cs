using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System;

namespace front.Pages
{
    public class TestModel : PageModel
    {
        [BindProperty] public List<MyChartTestModel> datalist { get; set; }
		public List<string> xd = new List<string>();
		public List<int> xdd =  new List<int>();
        public void OnGet()
        {
			int x = 0;
            datalist = new List<MyChartTestModel>();
            datalist.Add(new MyChartTestModel {MyLabel = "Red", MyData = "12"});
            datalist.Add(new MyChartTestModel {MyLabel = "Blue", MyData = "19"});
            datalist.Add(new MyChartTestModel {MyLabel = "Yellow", MyData = "3"});
            datalist.Add(new MyChartTestModel {MyLabel = "Green", MyData = "5"});
            datalist.Add(new MyChartTestModel {MyLabel = "Purple", MyData = "2"});
            datalist.Add(new MyChartTestModel {MyLabel = "Orange", MyData = "3"});
			foreach(var i in datalist){
				xd.Add(i.MyLabel);
				xdd.Add(x);
				x += 2;
			}
        }
    }

    public class MyChartTestModel
    {
        public string MyData { get; set; }
        public string MyLabel { get; set; }
    }
}