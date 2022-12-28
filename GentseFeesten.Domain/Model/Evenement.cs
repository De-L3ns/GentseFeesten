using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class Evenement
    {
        private List<Evenement> _childEvents = new List<Evenement>();
        public Evenement(string id, string name, List<string?> childEventIds, DateTime? start, DateTime? end, string description, int? price)
        {
            Id = id;
            Name = name;
            Start = start;
            End = end;
            Description = description;
            Price = price;
            ChildEventIds = childEventIds;
        }

        public string Id { get; init; }
        public string Name { get; init; }
        public List<string?> ChildEventIds { get; init; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string StartDateToDatabase { get => Start?.ToString("yyyy-MM-dd HH:mm:ss"); }
        public string EndDateToDatabase { get => End?.ToString("yyyy-MM-dd HH:mm:ss"); }
        public string Description { get; set; }
        public int? Price { get; set; }


        public void SetChilds(List<Evenement> childEvents)
        {
            _childEvents = childEvents;
        }
        public List<Evenement> GetChilds()
        {
            return _childEvents;
        }
        
        public override string? ToString()
        {
            return $"Naam: {this.Name}\nStart: {this.Start}\nEinde: {this.End}\nBeschrijving: {this.Description}\n €{this.Price}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Evenement evenement && Id.Equals(evenement.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
