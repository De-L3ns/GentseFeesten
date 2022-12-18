using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class ParentEvenement : Evenement
    {
        public List<string> childEventIds { get; init; }
        private List<Evenement> _childEvenementen;
        public ParentEvenement(string id, string naam, DateTime? start, DateTime? einde, string beschrijving, int? prijs, List<string> childEventIds) : base(id, naam, start, einde, beschrijving, prijs)
        {
            this.childEventIds = childEventIds;
        }

        public void AddChilds(List<Evenement> childs) 
        {
            childs.ForEach(child => { _childEvenementen.Add(child); });
        }

        public List<Evenement> RetrieveChilds()
        {
            return _childEvenementen;
        }

        public override string? ToString()
        {
            List<string> childNames = _childEvenementen.Select(child => child.Naam).ToList();
            return base.ToString() + $"{string.Join("\n-", childNames)}";
        }


    }
}
