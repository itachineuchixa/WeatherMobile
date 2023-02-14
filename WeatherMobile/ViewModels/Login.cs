using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherMobile.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace WeatherMobile.ViewModels
{

    public class Login: INotifyPropertyChanged
    {
        string Results = "";
        static async Task<string> Auth(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            var dt = DateTime.Now;
            DateTime month = dt.AddMonths(-1);
            // получаем ответ
            using HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7205/login/" + username + "/" + password);
            return await response.Content.ReadAsStringAsync();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        DataLogin datal =  new DataLogin();
        public ICommand LogClck { get; }

        public INavigation Navigation { get; set; }

        public ICommand RegClck { get; }

        public Login()
        {
            LogClck = new Command(Log);
            RegClck = new Command(Reg);
            /*ShowMoreInfoCommand = new AsyncRelayCommand(Auth);*/
        }

        public string username
        {
            get { return datal.username; }
            set
            {
                if (datal.username != value)
                {
                    datal.username = value;
                    OnPropertyChanged("userrname");
                }
            }
        }

        public string password
        {
            get { return datal.password; }
            set
            {
                if (datal.password != value)
                {
                    datal.password = value;
                    OnPropertyChanged("password");
                }
            }
        }

        public string RESULTS
        {
            get { return Results; }
            set
            {
                if (Results != value)
                {
                    Results = value;
                    OnPropertyChanged("RESULTS");
                }
            }
        }

        public async void Log()
        {
            var response = await Auth(datal.username, datal.password);
            if (response.ToString() == "Success")
            {
                User.Password = datal.password;
                User.Username = datal.username;
                RESULTS= "Success";
                Navigation.PushAsync(new DataPage());
                /*manager.MainFrame.Navigate(new Weath());*/
            }
            else { /*await Application.Current.MainPage.DisplayAlert("Неверные данные!");*/
                RESULTS = response.ToString();
            };
        }

        public async void Reg()
        {
            Navigation.PushAsync(new RegPage());
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}

