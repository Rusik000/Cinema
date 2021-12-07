using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Movie
    {

        public string Name { get; set; }
        public string Poster { get; set; }
        public DateTime Year { get; set; }
        public string Imdb { get; set; }

        public string Country   { get; set; }

        public string Writers   { get; set; }

        public string Director  { get; set; }

        public string MovieImage { get; set; }

        public string MovieImage2 { get; set; }
        public string Minute { get; set; }

        public string Genre { get; set; }
        public string Plot { get; set; }
        public string Time { get; set; }
        public string Hall { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string Hdate { get; set; }
        
        public List<CheckBoxx> Seats { get; set; } = new List<CheckBoxx>();

    }

}
