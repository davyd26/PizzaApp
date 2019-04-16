using Newtonsoft.Json;
using PizzaApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzaApp
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();

            maListePizzas.RefreshCommand = new Command(() =>
            {
                maListePizzas.IsRefreshing = true;
                RefreshList();
                maListePizzas.IsRefreshing = false;
            });

            RefreshList();
        }

        private void RefreshList()
        {
            maListePizzas.IsVisible = false;
            waitLayout.IsVisible = true;

            using (var webClient = new WebClient())
            {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                    webClient.DownloadStringAsync(new Uri("https://drive.google.com/uc?export=download&id=1a4_-xGB39MvOcN_IybfHlDv7tlDo7l5j"));

            }
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string pizzasStr = e.Result;
                List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasStr);

                Device.BeginInvokeOnMainThread(() =>
                {
                    maListePizzas.ItemsSource = pizzas;
                    maListePizzas.IsVisible = true;
                    waitLayout.IsVisible = false;
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Erreur", "Une erreur s'est produite : " + ex.Message, "OK");
                });
            }

        }

    }
}
