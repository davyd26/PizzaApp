using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApp.Model
{
    class Pizza
    {
        public string Nom { get; set; }
        public int Prix { get; set; }
        public List<string> Ingredients { get; set; }
    }
}
