using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class ChildEvenement : Evenement
    {
        public List<string?> childEventIds { get; init; }
        public string? parentId { get; init; }
        public ChildEvenement(string id, string naam, DateTime? start, DateTime? einde, string beschrijving, int? prijs, List<string?> childEventIds, string? parentId) : base(id, naam, start, einde, beschrijving, prijs)
        {
            this.childEventIds = childEventIds;
            this.parentId = parentId;
        }

        public override string? ToString()
        {
            return base.ToString() + " I am a child Event";
        }
    }
}
