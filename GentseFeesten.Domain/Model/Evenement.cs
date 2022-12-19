using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class Evenement
    {
        public Evenement(string id, string naam, DateTime? start, DateTime? einde, string beschrijving, int? prijs)
        {
            Id = id;
            Naam = naam;
            Start = start;
            Einde = einde;
            Beschrijving = beschrijving;
            Prijs = prijs;
        }

        public string Id { get; init; }
        public string Naam { get; init; }

        public DateTime? Start { get; set; }
        public DateTime? Einde { get; set; }
        public string Beschrijving { get; set; }
        public int? Prijs { get; set; }
       

        public override string? ToString()
        {
            return $"Naam: {this.Naam}\nStart: {this.Start}\nEinde: {this.Einde}\nBeschrijving: {this.Beschrijving}\n €{this.Prijs} ";
        }

    }
}
