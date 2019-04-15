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
        List<Pizza> pizzas = null;

        public MainPage()
        {
            InitializeComponent();

            string pizzasStr = string.Empty;
            using (var webClient = new WebClient())
            {
                try
                {
                    pizzasStr = webClient.DownloadString("https://drive.google.com/uc?export=download&id=1a4_-xGB39MvOcN_IybfHlDv7tlDo7l5j");
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Erreur", "Une erreur s'est produite : " + ex.Message, "OK");
                    });
                }
            }

            pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasStr);

            maListePizzas.ItemsSource = pizzas;
        }
    }
}
