using PizzaApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

            pizzas = new List<Pizza>
            {
                new Pizza() { Nom = "Végétarienne", Ingredients = new List<string>() { "tomate", "poivrons", "oignons" }, Prix = 7 },
                new Pizza() { Nom = "Montagnarde", Ingredients = new List<string>() { "reblochon", "pomme de terre", "oignons", "crème fraiche" }, Prix = 11 },
                new Pizza() { Nom = "Carnivore", Ingredients = new List<string>() { "fromage de chèvre", "oignons", "crème fraiche", "thym" }, Prix = 12 }
            };

            //maListePizzas.
        }
    }
}
