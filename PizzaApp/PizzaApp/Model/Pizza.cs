using System;
using System.Collections.Generic;
using System.Text;
using PizzaApp.extensions;

namespace PizzaApp.Model
{
    class Pizza
    {
        public string Nom { get; set; }
        public string ImageUrl { get; set; }
        public int Prix { get; set; }
        public List<string> Ingredients { get; set; }
        public string PrixEuros {
            get 
            {
                return string.Format("{0:0} €", Prix);
            }
        }
        public string IngredientsStr
        {
            get
            {
                return string.Join(", ", Ingredients);
            }
        }
        public string Titre { get { return Nom.PremiereLettreMajuscule(); } }
    }
}
