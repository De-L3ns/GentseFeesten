using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class Evenement
    {
        public Evenement(string id, string name, DateTime? start, DateTime? end, string description, int? price)
        {
            Id = id;
            Name = name;
            Start = start;
            End = end;
            Description = description;
            Price = price;
        }

        public string Id { get; init; }
        public string Name { get; init; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
       

        public override string? ToString()
        {
            return $"Naam: {this.Name}\nStart: {this.Start}\nEinde: {this.End}\nBeschrijving: {this.Description}\n €{this.Price}";
        }

    }
}
