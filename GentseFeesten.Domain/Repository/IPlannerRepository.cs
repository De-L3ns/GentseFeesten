using GentseFeesten.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeesten.Domain.Repository
{
    public interface IPlannerRepository
    {
        public List<Evenement> GetAllEventsOnPlanner();
        public int GetCurrentTotalPrice();
        public void AddEventToPlanner(Evenement evenement);
        public void RemoveEventFromPlanner(Evenement evenement);

    }
}
