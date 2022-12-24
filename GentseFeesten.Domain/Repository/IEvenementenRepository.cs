using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Repository
{
    public interface IEvenementenRepository
    {
        public List<Evenement> GetAllParentEvents();
        public List<Evenement> GetChilds(Evenement evenement);
        public List<DateTime?> GetMissingStartData(Evenement evenement);
        public List<DateTime?> GetMissingEndData(Evenement evenement);
        public int GetMissingPriceData(Evenement evenement);
        public Evenement GetEventById(string id);
    }
}
