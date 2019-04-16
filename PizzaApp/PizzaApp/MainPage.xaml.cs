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
    public enum E_tri
    {
        TRI_AUCUN,
        TRI_NOM,
        TRI_PRIX
    }

    public partial class MainPage : ContentPage
    {
        public E_tri current_tri = E_tri.TRI_AUCUN;

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

        public string GetImageSourceFromTri(E_tri t)
        {
            string retValue = "sort_none.png";

            switch (t)
            {
                case E_tri.TRI_AUCUN:
                    retValue = "sort_none.png";
                    break;
                case E_tri.TRI_NOM:
                    retValue = "sort_nom.png";
                    break;
                case E_tri.TRI_PRIX:
                    retValue = "sort_prix.png";
                    break;
                default:
                    retValue = "sort_none.png";
                    break;
            }

            return retValue;
        }

        public List<Pizza> GetPizzasFromTri(E_tri t, List<Pizza> l)
        {
            List<Pizza> retValue = null;
            List<Pizza> newList = new List<Pizza>(l);

            switch (t)
            {
                case E_tri.TRI_AUCUN:
                    retValue = newList;
                    break;
                case E_tri.TRI_NOM:
                    retValue = newList.OrderBy(f => f.Titre).ToList<Pizza>();
                    break;
                case E_tri.TRI_PRIX:
                    retValue = newList.OrderByDescending(f => f.Prix).ToList<Pizza>();
                    break;
                default:
                    retValue = newList;
                    break;
            }

            return retValue;
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
                    maListePizzas.ItemsSource = GetPizzasFromTri(current_tri, pizzas);
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

        private void TriButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("TriButton_Clicked");

            switch (current_tri)
            {
                case E_tri.TRI_AUCUN:
                    current_tri = E_tri.TRI_NOM;
                    break;
                case E_tri.TRI_NOM:
                    current_tri = E_tri.TRI_PRIX;
                    break;
                case E_tri.TRI_PRIX:
                    current_tri = E_tri.TRI_AUCUN;
                    break;
                default:
                    current_tri = E_tri.TRI_AUCUN;
                    break;
            }

            TriButton.Source = GetImageSourceFromTri(current_tri);
            RefreshList();
        }
    }
}
