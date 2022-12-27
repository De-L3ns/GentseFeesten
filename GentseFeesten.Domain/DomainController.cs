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
            List<Evenement> mainEvents = _evenementenRepository.GetMainEvents();
            return mainEvents;

        }

        public string GetEventDetails(string id)
        {
            Evenement evenement = GetEventById(id);
            FillInMissingDate(evenement);
            
            return evenement.ToString();
        }

        public List<Evenement> GetChildsFromEvent (string id)
        {
            Evenement evenement = GetEventById(id);
            _evenementenRepository.GetChildEvents(evenement);
            List<Evenement> childEvents = evenement.GetChilds();
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
            
            if (evenement.End == null)
            {
                List<DateTime?> endDates = _evenementenRepository.GetMissingEndData(evenement).OrderByDescending(x => x).ToList();
                evenement.End = endDates.First();
            }
            
            if (evenement.Price == null)
            {
                evenement.Price = _evenementenRepository.GetMissingPriceData(evenement);
            }
        }

        private Evenement GetEventById(string id)
        {
            Evenement evenement = _evenementenRepository.GetEventById(id);
            return evenement;
        }
    }
}