using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class CheckBoxx
    {

        public CheckBoxx(string name, bool? isChecked, double price)
        {
            Name = name;
            IsChecked = (bool)isChecked;
            Price = price;
        }

        public string Name { get; set; }
        public bool IsChecked { get; set; } = false;
        public bool IsPressed { get; set; } = false;
        public double Price { get; set; }
    }
}
