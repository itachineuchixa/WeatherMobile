using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Runtime.Intrinsics.X86;
using Microsoft.Maui.ApplicationModel;

namespace WeatherMobile
{
    class ViewModelDataPage: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        public ICommand Goto { get; }

        public ICommand GotoAd { get; }

        public ICommand Delete { get; }

        List<Cities> citi = new List<Cities>();

        private async void fnstart()
        {
            var user_id = await GetId(User.Username, User.Password);
            var user_cities = await Find(user_id.ToString());
            citi = JsonConvert.DeserializeObject<List<Cities>>(user_cities);
            for (int i = 0; i < citi.Count; i++)
            {
                it.Add(citi[i].City1);
            }
            PickerItems = it;
        }
        static async Task<string> Find(string user_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_cities/" + user_id);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> GetId(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_user_id/" + username + "/" + password);
            return await response.Content.ReadAsStringAsync();
        }


        static async Task<string> Get_current(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_current/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }


        static async Task<string> Get_min(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_minimum/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }

        static async Task<string> Get_max(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_maximum/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> Get_graph(string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_graph/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }
        public ObservableCollection<string> PickerItems { get; set; }

        public ObservableCollection<string> it = new ObservableCollection<string>();

        public string current_temp = "";

        public string min_temp = "0 °c";


        public string max_temp = "";


        public string minn_temp = "";


        public string avg_temp = "";
        public string SelectedPickerItem { get; set; }

        public string curr_mode { get; set; }

        public ObservableCollection<string> ModeItems { get; set; }

        public ViewModelDataPage()
        {
            Goto = new Command(GotoGraph);
            GotoAd = new Command(GotoAdd);
            Delete = new Command(Delete_Click);
            fnstart();
            PickerItems = it;
            ModeItems = new ObservableCollection<string> { "Week", "Month" };

        }
        public async void settemp()
        {
            if (Models.Mode.Current_mode != null)
            {
                long id = (from i in citi where i.City1 == RESULTS select i.Id).ToList()[0];
                Models.Mode.City_id = id.ToString();
                if (Models.Mode.Current_mode == "Month")
                {
                    Current_Weather = await Get_current(id.ToString()) + "°c";
                    var min = await Get_min(id.ToString());
                    string[] mins = min.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] mini = new double[mins.Count()];
                    for (int i = 0; i < mins.Count(); i++)
                    {
                        mini[i] = double.Parse(mins[i]);
                    }
                    Min_Weather = mini.Min().ToString() + "°c";
                    var max = await Get_max(id.ToString());
                    string[] maxs = max.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] maxi = new double[mins.Count()];
                    for (int i = 0; i < maxs.Count(); i++)
                    {
                        maxi[i] = double.Parse(maxs[i]);
                    }
                    Max_Weather = maxi.Max().ToString() + "°c";
                    var avg = await Get_graph(id.ToString());
                    string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] av = new double[avgs.Count()];
                    for (int i = 0; i < avgs.Count(); i++)
                    {
                        av[i] = double.Parse(avgs[i]);
                    }
                    double a = av.Average();
                    Avg_Weather = Math.Round(a, 2).ToString() + "°c";
                }
                else
                {
                    Current_Weather = await Get_current(id.ToString()) + "°c";
                    var min = await Get_min(id.ToString());
                    string[] mins = min.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] mini = new double[7];
                    int n = 0;
                    for (int i = mins.Count() - 7; i < mins.Count(); i++)
                    {
                        mini[n] = double.Parse(mins[i]);
                        n++;
                    }
                    Min_Weather = mini.Min().ToString() + "°c";
                    var max = await Get_max(id.ToString());
                    string[] maxs = max.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] maxi = new double[7];
                    n = 0;
                    for (int i = maxs.Count() - 7; i < maxs.Count(); i++)
                    {
                        maxi[n] = double.Parse(maxs[i]);
                        n++;
                    }
                    Max_Weather = maxi.Max().ToString() + "°c";
                    var avg = await Get_graph(id.ToString());
                    string[] avgs = avg.Replace(",", ";").Replace(".", ",").Split("[")[1].Split("]")[0].Split(";");
                    double[] av = new double[160];
                    n = 0;
                    for (int i = avgs.Count() - 160; i < avgs.Count(); i++)
                    {
                        av[n] = double.Parse(avgs[i]);
                        n++;
                    }
                    double a = av.Average();
                    Avg_Weather = Math.Round(a, 2).ToString() + "°c";
                }
            }
        }

        private async void Delete_Click()
        {
            {// удаление нескольких пользователе
                bool Answer = await Application.Current.MainPage.DisplayAlert("Подтвердить действие", "Вы хотите удалить элемент?", "Да", "Нет");
                Min_Weather = Answer.ToString();
                if (Answer)
                {

                    try
                    {
                        var user_id = await GetId(User.Username, User.Password);
                        var result = await Del(user_id, Models.Mode.City_id);
                        if (result == "Success") await Application.Current.MainPage.DisplayAlert("Успех", "Данные удалены", "ОК");
                        else await Application.Current.MainPage.DisplayAlert("Неудача", result+ user_id+ Models.Mode.City_id, "ОК");
                        fnstart();
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message.ToString(), "ОК");
                    }
                }


            }
        }

        static async Task<string> Del(string user_id, string city_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/delete/" + user_id + "/" + city_id);
            return await response.Content.ReadAsStringAsync();
        }

        public  string RESULTS
        {
            get {
                return SelectedPickerItem; }
            set
            {
                if (SelectedPickerItem != value)
                {
                    SelectedPickerItem = value;
                    OnPropertyChanged("RESULTS");
                    settemp();
                }
            }
        }

        public string SETMODE
        {
            get
            {
                return curr_mode;
            }
            set
            {
                if (curr_mode != value)
                {
                    curr_mode = value;
                    Models.Mode.Current_mode = value;
                    OnPropertyChanged("SETMODE");
                    settemp();
                }
            }
        }

        public string Current_Weather
        {
            get { return current_temp; }
            set
            {
                if (current_temp != value)
                {
                    current_temp = value;
                    OnPropertyChanged("Current_Weather");
                }
            }
        }

        public string Min_Weather
        {
            get { return min_temp; }
            set
            {
                if (min_temp != value)
                {
                    min_temp = value;
                    OnPropertyChanged("Min_Weather");
                }
            }
        }

        public string Max_Weather
        {
            get { return max_temp; }
            set
            {
                if (max_temp != value)
                {
                    max_temp = value;
                    OnPropertyChanged("Max_Weather");
                }
            }
        }


        public string Avg_Weather
        {
            get { return avg_temp; }
            set
            {
                if (avg_temp != value)
                {
                    avg_temp = value;
                    OnPropertyChanged("Avg_Weather");
                }
            }
        }


        public async void GotoGraph()
        {
            if (Models.Mode.Current_mode != null)
                Navigation.PushAsync(new Weath());
        }

        public async void GotoAdd()
        {
            Navigation.PushAsync(new AddCityPage());
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
