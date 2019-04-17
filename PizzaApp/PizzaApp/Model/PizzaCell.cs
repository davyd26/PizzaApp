using System;
using System.ComponentModel;
using System.Windows.Input;
using PizzaApp.extensions;
using Xamarin.Forms;

namespace PizzaApp.Model
{
    public class PizzaCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Pizza pizza { get; set; }
        public bool isFavorite { get; set; }

        public string ImageSourceFav { get { return isFavorite ? "star2.png" : "star1.png";  } }

        public ICommand FavClickCommand { get; set; }

        public Action<PizzaCell> FavChangedAction { get; set; }

        public PizzaCell()
        {
            FavClickCommand = new Command((obj) =>
            {
                Pizza param = obj as Pizza;

                isFavorite = !isFavorite;

                OnPropertyChanged("ImageSourceFav");

                FavChangedAction.Invoke(this);
            });
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
