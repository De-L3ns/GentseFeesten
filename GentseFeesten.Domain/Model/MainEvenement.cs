using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Model
{
    public class MainEvenement : Evenement
    {
        
        public MainEvenement(string id, string name, List<string?> childEventIds, DateTime? start, DateTime? end, string description, int? price) : base(id, name, childEventIds, start, end, description, price)
        {
            
        }
        public override string? ToString()
        {
            return base.ToString();
        }

    }
}
