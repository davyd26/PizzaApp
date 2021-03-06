﻿using Newtonsoft.Json;
using PizzaApp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        TRI_PRIX,
        TRI_FAV
    }

    public partial class MainPage : ContentPage
    {
        public E_tri current_tri = E_tri.TRI_AUCUN;

        const string KEY_TRI = "tri";
        const string KEY_FAV = "fav";

        string tempFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp");
        string jsonFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pizzas.json");

        List<string> PizzasFav = new List<string>();

        public MainPage()
        {
            InitializeComponent();

            LoadFavList();

            if (Application.Current.Properties.ContainsKey(KEY_TRI))
            {
                current_tri = (E_tri)Application.Current.Properties[KEY_TRI];
                TriButton.Source = GetImageSourceFromTri(current_tri);
            }

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
                case E_tri.TRI_FAV:
                    retValue = "sort_fav.png";
                    break;
                default:
                    retValue = "sort_none.png";
                    break;
            }

            return retValue;
        }

        public void SaveFavList()
        {
            Application.Current.Properties[KEY_FAV] = string.Join(",", PizzasFav.ToArray<string>());
        }

        public void LoadFavList()
        {
            if (Application.Current.Properties.ContainsKey(KEY_FAV))
            {
                PizzasFav = Application.Current.Properties[KEY_FAV].ToString().Split(',').ToList();
            }
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
                case E_tri.TRI_FAV:
                    retValue = newList.Where(s => PizzasFav.Contains(s.Nom) == true).OrderBy(f => f.Nom).ToList<Pizza>();
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
                    maListePizzas.ItemsSource = GetPizzaCells(GetPizzasFromTri(current_tri, pizzas), PizzasFav);
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

        private void OnFavPizzaChanged(PizzaCell pizzaCell)
        {
            bool isinFavList = PizzasFav.Contains(pizzaCell.pizza.Nom);

            if (pizzaCell.isFavorite && !isinFavList)
                PizzasFav.Add(pizzaCell.pizza.Nom);
            else if (!pizzaCell.isFavorite && isinFavList)
                PizzasFav.Remove(pizzaCell.pizza.Nom);

            SaveFavList();
            RefreshList();
        }

        private List<PizzaCell> GetPizzaCells(List<Pizza> p, List<string> f)
        {
            List<PizzaCell> ret = new List<PizzaCell>();

            if (p == null)
            {
                return ret;
            }

            foreach (Pizza pizza in p)
            {
                bool isFav = f.Contains(pizza.Nom);

                ret.Add(new PizzaCell { pizza = pizza, isFavorite = isFav, FavChangedAction = OnFavPizzaChanged });
            }

            return ret;
        }

        private void TriButton_Clicked(object sender, EventArgs e)
        {
            switch (current_tri)
            {
                case E_tri.TRI_AUCUN:
                    current_tri = E_tri.TRI_NOM;
                    break;
                case E_tri.TRI_NOM:
                    current_tri = E_tri.TRI_PRIX;
                    break;
                case E_tri.TRI_PRIX:
                    current_tri = E_tri.TRI_FAV;
                    break;
                case E_tri.TRI_FAV:
                    current_tri = E_tri.TRI_AUCUN;
                    break;
                default:
                    current_tri = E_tri.TRI_AUCUN;
                    break;
            }

            Application.Current.Properties[KEY_TRI] = (int)current_tri;
            Application.Current.SavePropertiesAsync();

            TriButton.Source = GetImageSourceFromTri(current_tri);
            RefreshList();
        }
    }
}
