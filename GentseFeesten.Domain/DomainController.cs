using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using System.Diagnostics;

namespace GentseFeesten.Domain
{
    public class DomainController
    {
        private readonly IEvenementenRepository _evenementenRepository;

        public DomainController(IEvenementenRepository evenementenRepository)
        {
            _evenementenRepository = evenementenRepository;
        }


        public List<Evenement> GetAllMainEvents()
        {
            List<Evenement> mainEvents = _evenementenRepository.GetAllParentEvents();
            return mainEvents;

        }

        public string ShowEventDetails(string id)
        {
            Evenement eventToShowDetailsFrom = GetEventById(id);
            FillInMissingDate(eventToShowDetailsFrom);
            return $"{eventToShowDetailsFrom}";

        }

        public List<Evenement> GetChildsFromEvent (string id)
        {
            Evenement evenement = GetEventById(id);
            List<Evenement> childEvents = _evenementenRepository.GetChilds(evenement);
            foreach (Evenement childEvent in childEvents)
            {
                FillInMissingDate(childEvent);
            }
            return childEvents;

        }

        private void FillInMissingDate(Evenement evenement)
        {
            if (evenement.Start == null)
            {
                List<DateTime?> startDates = _evenementenRepository.GetMissingStartData(evenement).OrderBy(x => x).ToList();
                evenement.Start = startDates.First();
            }
            
            if (evenement.Einde == null)
            {
                List<DateTime?> endDates = _evenementenRepository.GetMissingEndData(evenement).OrderByDescending(x => x).ToList();
                evenement.Einde = endDates.First();
            }
            
            if (evenement.Prijs == null)
            {
                evenement.Prijs = _evenementenRepository.GetMissingPriceData(evenement);
            }
        }

        private Evenement GetEventById(string id)
        {
            Evenement evenement = _evenementenRepository.GetEventById(id);
            return evenement;
        }
    }
}