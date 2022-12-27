using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class ChildEvenement : Evenement
    {
        private Evenement _mainEvent;
        
        public ChildEvenement(string id, string naam, DateTime? start, DateTime? einde, string beschrijving, int? prijs, List<string?> childEventIds, string? parentId, Evenement mainEvent) : base(id, naam, childEventIds, start, einde, beschrijving, prijs)
        {
            ParentId = parentId;
            _mainEvent = mainEvent;
        }

        public string? ParentId { get; init; }

        public override string? ToString()
        {
            return $"Parent: {_mainEvent.Name}\n" + base.ToString();
        }
    }
}
