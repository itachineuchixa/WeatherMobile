using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherMobile.ViewModels
{
    public class AddCity: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string txtsrch;

        public string slctditem;

        public INavigation Navigation { get; set; }

        public ObservableCollection<string> City_name = new ObservableCollection<string>();
        public ObservableCollection<string> PickerItems { get; set; }
        public List<Cities> city_list = new List<Cities>();
        public ICommand Finds { get; }

        public AddCity()
        {
            Finds = new Command(BtnFind_Click);
        }

        public string Search
        {
            get { return txtsrch; }
            set
            {
                if (txtsrch != value)
                {
                    txtsrch = value;
                    OnPropertyChanged("Search");
                }
            }
        }


        private async void BtnFind_Click()
        {
            string cit = Search;
            var response = await Find(cit);
            Search = response;
            Dictionary<string, string[]> citi = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(response);
            foreach (var citation in citi)
            {
                Cities curr = new Cities();
                curr.City1 = citation.Value[0];
                curr.Longitude = double.Parse(citation.Value[1].Replace(".", ","));
                curr.Latitude = double.Parse(citation.Value[2].Replace(".", ","));
                city_list.Add(curr);
            }
            foreach (var city in city_list) City_name.Add(city.City1);
            Search = City_name[0];
            PickerItems = City_name;
            Adds_Click();



        }
        private async void Adds_Click()
        {
            var current_city = city_list[0];
            var user_id = await GetId(User.Username, User.Password);
            var response = await AddTown(current_city.City1, current_city.Longitude.ToString(), current_city.Latitude.ToString(), user_id);
            if (response == "Success")
            {
                MessagingCenter.Send(this, "Город успешно добавлен");
                Navigation.PushAsync(new DataPage());
            }
            else if (response == "City already exist")
            {
                MessagingCenter.Send(this, "Город успешно добавлен");
                Navigation.PushAsync(new DataPage());
            }
            else
            {
                MessagingCenter.Send(this,"Произошла ошибка!");
            }

        }
        static async Task<string> AddTown(string city, string longtitude, string latitude, string user_id)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/add_city/" + city + "/" + longtitude + "/" + latitude + "/" + user_id);
            return await response.Content.ReadAsStringAsync();
        }

        static async Task<string> GetId(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_user_id/" + username + "/" + password);
            return await response.Content.ReadAsStringAsync();
        }
        static async Task<string> Find(string city)
        {
            HttpClient httpClient = new HttpClient();
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/get_geo/" + city);
            return await response.Content.ReadAsStringAsync();
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
