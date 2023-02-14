using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using WeatherMobile.Models;

namespace WeatherMobile
{
    [ObservableObject]
    public partial class ViewModel
    {
        
        static async Task<string> Get_graph(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_graph/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }
        public ViewModel()
        {
            /*SeriesCollection = new ISeries[] { new LineSeries<double> { Values = new double[] {-10, -5, 1, 2, 3, 6, 7, 8, 9, 10 } } };*/
            get_data();
            SeriesCollection = new ISeries[] { new LineSeries<double> { Values = dataY } };
        }
        public async void get_data()
        {
            if (Mode.Current_mode == "Week")
            {
                var avg = await Get_graph(Mode.City_id);
                string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                double[] av = new double[160];
                int n = 0;
                List<List<double>> datasets = new List<List<double>>();
                for (int i = 0; i < 7; i++) datasets.Add(new List<double>());

                for (int i = avgs.Count() - 160; i < avgs.Count(); i++)
                {
                    datasets[n].Add(double.Parse(avgs[i]));
                    if (i % 24 == 0) n++;
                    /*dataY.Add(double.Parse(avgs[i]));*/
                }
                List<double> avg_temps = new List<double>();
                for (int i = 0; i < datasets.Count(); i++)
                {
                    avg_temps.Add(datasets[i].Average());
                }
                foreach (double data in avg_temps) dataY.Add(data);
                /*dataY = av.ToList();*/
                SeriesCollection = new ISeries[] { new LineSeries<double> { Values = new double[] { 1, 2, 3, 4, 5, 6 } } };
                var values = new int[100];
            }
            else
            {
                var avg = await Get_graph(Mode.City_id);
                string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                double[] av = new double[avgs.Count()];
                int n = 0;
                List<List<double>> datasets = new List<List<double>>();
                for (int i = 0; i < 32; i++) datasets.Add(new List<double>());

                for (int i = 0; i < 730; i++)
                {
                    datasets[n].Add(double.Parse(avgs[i]));
                    if (i % 24 == 0) n++;
                    /*dataY.Add(double.Parse(avgs[i]));*/
                }
                List<double> avg_temps = new List<double>();
                for (int i = 0; i < datasets.Count(); i++)
                {
                    avg_temps.Add(datasets[i].Average());
                }
                foreach (double data in avg_temps) dataY.Add(data);
                /*dataY = av.ToList();*/
                SeriesCollection = new ISeries[] { new LineSeries<double> { Values = new double[] { 1, 2, 3, 4, 5, 6 } } };
                var values = new int[100];
            }
        }

        public ISeries[] SeriesCollection { get; set; }
        public List<double> dataY = new List<double>();

    }
}